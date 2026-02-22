using Fodo.Application.Implementation.Interfaces;
using Fodo.Application.Implementation.IRepositories;
using Fodo.Contracts.DTOS;
using Fodo.Contracts.Responses;
using Fodo.Domain.Entities;

namespace Fodo.Application.Implementation.Services
{
    public class BranchesService : IBranchesService
    {
        private readonly IClientRepository _clientRepo;
        private readonly IBranchesRepository _repo;
        public BranchesService(IClientRepository clientRepo, IBranchesRepository branchRepo)
        {
            _clientRepo = clientRepo;
            _repo = branchRepo;
        }

        public async Task<ClientsDto> GetByClientCodeAsync(string clientCode, CancellationToken ct)
        {
            var clientId = await _clientRepo.GetClientIdByCodeAsync(clientCode, ct);
            if (clientId is null) return null;

            var branches = await _repo.GetByClientIdAsync(clientId.Value, ct);
            return new ClientsDto(clientId.Value, branches);
        }

        public async Task<BranchCatalogResponse> GetBranchCatalogAsync(int branchId)
        {
            if (branchId <= 0)
                return new BranchCatalogResponse { Success = false, Message = "branchId is required." };

            // Load branch
            var branch = await _repo.GetBranchAsync(branchId);
            if (branch == null)
                return new BranchCatalogResponse { Success = false, Message = "Branch not found." };

            int clientId = branch.ClientId;

            // CategoryBranches → Categories
            var categoryBranches = await _repo.GetCategoryBranchesByBranchAsync(branchId);

            var categoryIds = categoryBranches
                .Select(x => x.CategoryId)
                .Distinct()
                .ToList();

            var categories = await _repo.GetCategoriesByIdsAsync(categoryIds);

            // Items under categories
            var items = await _repo.GetItemsByCategoryIdsAsync(categoryIds);
            var itemIds = items.Select(i => i.ItemId).Distinct().ToList();

            // Prices per item
            var itemPricelists = await _repo.GetItemPricelistsByItemIdsAsync(itemIds);

            // Shared lookups
            var priceLists = await _repo.GetPriceListsByClientAsync(clientId);
            var taxRules = await _repo.GetTaxRulesByClientAsync(clientId);

            // ====== NEW: Load modifiers graph in bulk ======

            // 1) Links Item -> ModifierGroup (has sequence)
            var itemModifierGroups = await _repo.GetItemModifierGroupsByItemIdsAsync(clientId,itemIds);

            // filter active links (if your table has IsActive)
            var activeItemGroupLinks = itemModifierGroups
                .Where(x => x.IsActive)
                .ToList();

            var modifierGroupIds = activeItemGroupLinks
                .Select(x => x.ModifierGroupId)
                .Distinct()
                .ToList();

            // 2) ModifierGroups
            var modifierGroups = modifierGroupIds.Count == 0
                ? new List<ModifiersGroup>()
                : await _repo.GetModifierGroupsByIdsAsync(clientId,modifierGroupIds);

            var activeGroups = modifierGroups.Where(g => g.IsActive).ToList();

            // 3) Modifiers (options under groups)
            var activeGroupIds = activeGroups.Select(g => g.Id).ToList();

            var modifiers = activeGroupIds.Count == 0
                ? new List<Modifiers>()
                : await _repo.GetModifiersByGroupIdsAsync(clientId,activeGroupIds);

            var activeModifiers = modifiers.Where(m => m.IsActive).ToList();
            var modifierIds = activeModifiers.Select(m => m.Id).Distinct().ToList();

            // 4) Modifier price overrides per pricelist
            var modifierPricelists = modifierIds.Count == 0
                ? new List<ModifiersPricelist>()
                : await _repo.GetModifierPricelistsByModifierIdsAsync(modifierIds);

            // ====== Build lookups (items + prices + modifiers) ======

            // Group for nesting
            var itemsByCategory = items
                .GroupBy(x => x.CategoryId)
                .ToDictionary(g => g.Key, g => g.ToList());

            var priceListLookup = priceLists.ToDictionary(
                x => x.PriceListId,
                x => new { x.NameEn, x.NameAr }
            );

            var pricesByItem = itemPricelists
                .GroupBy(x => x.ItemId)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(p =>
                    {
                        priceListLookup.TryGetValue(p.PricelistId, out var pl);

                        return new ItemPricesDto
                        {
                            PricelistId = p.PricelistId,
                            Price = p.Price,
                            PriceListNameEn = pl?.NameEn,
                            PriceListNameAr = pl?.NameAr
                        };
                    }).ToList()
                );

            var taxRuleLookup = taxRules.ToDictionary(
                x => x.TaxRuleId,
                x => new TaxRulesDto
                {
                    TaxRuleId = x.TaxRuleId,
                    NameEN = x.NameEN,
                    NameAR = x.NameAR,
                    Rate = x.Rate
                }
            );

            // -------- Modifiers lookups with sequencing --------

            // Item -> (GroupId, Sequence)
            var groupsByItem = activeItemGroupLinks
                .GroupBy(x => x.ItemId)
                .ToDictionary(
                    g => g.Key,
                    g => g
                        .OrderBy(x => x.SortOrder) // <-- group sequence for item
                        .ToList()
                );

            // GroupId -> Group entity
            var groupById = activeGroups.ToDictionary(g => g.Id);

            // GroupId -> Modifiers ordered by SortOrder
            var modifiersByGroup = activeModifiers
                .GroupBy(m => m.ModifierGroupId)
                .ToDictionary(
                    g => g.Key,
                    g => g.OrderBy(m => m.SortOrder).ToList()
                );

            // ModifierId -> Prices list (resolved with PriceList names)
            var modifierPricesByModifier = modifierPricelists
                .GroupBy(x => x.ModifierId)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(mp =>
                    {
                        priceListLookup.TryGetValue(mp.PriceListId, out var pl);

                        return new ModifiersPricesDto
                        {
                            PricelistId = mp.PriceListId,
                            Price = mp.Price,
                            PriceListNameEn = pl?.NameEn,
                            PriceListNameAr = pl?.NameAr
                        };
                    }).ToList()
                );

            // ====== Build nested categories/items ======

            var categoryNodes = new List<CategoriesDto>();

            foreach (var cat in categories)
            {
                var catNode = new CategoriesDto
                {
                    CategoryId = cat.CategoryId,
                    NameEn = cat.NameEn,
                    NameAr = cat.NameAr,
                    IsActive = cat.IsActive,
                    ClientId = cat.ClientId,
                    Items = new List<ItemsDto>()
                };

                if (itemsByCategory.TryGetValue(cat.CategoryId, out var catItems))
                {
                    foreach (var item in catItems)
                    {
                        // Build modifier groups for this item (ordered by sequence)
                        var itemModifierGroupDtos = new List<ModifiersGroupsDto>();

                        if (groupsByItem.TryGetValue(item.ItemId, out var itemGroupLinks))
                        {
                            foreach (var link in itemGroupLinks)
                            {
                                if (!groupById.TryGetValue(link.ModifierGroupId, out var grp))
                                    continue;

                                // modifiers for group (ordered)
                                modifiersByGroup.TryGetValue(grp.Id, out var grpModifiers);
                                grpModifiers ??= new List<Modifiers>();

                                var modifierDtos = grpModifiers.Select(m =>
                                {
                                    var priceDtos = modifierPricesByModifier.TryGetValue(m.Id, out var mPrices)
                                        ? mPrices
                                        : new List<ModifiersPricesDto>();

                                    return new ModifiersDto
                                    {
                                        ModifierId = m.Id,
                                        NameEn = m.NameEn,
                                        NameAr = m.NameAr,
                                        BasePrice = m.BasePrice,
                                        IsActive = m.IsActive,
                                        Sequence = m.SortOrder,
                                        Prices = priceDtos
                                    };
                                }).ToList();

                                itemModifierGroupDtos.Add(new ModifiersGroupsDto
                                {
                                    ModifierGroupId = grp.Id,
                                    NameEn = grp.NameEn,
                                    NameAr = grp.NameAr,
                                    MinSelect = grp.MinSelect,
                                    MaxSelect = grp.MaxSelect,
                                    IsRequired = grp.IsRequired,
                                    IsActive = grp.IsActive,
                                    ClientId = grp.ClientId,
                                    Sequence = link.SortOrder, // <-- from ItemModifierGroups
                                    Modifiers = modifierDtos
                                });
                            }
                        }

                        catNode.Items.Add(new ItemsDto
                        {
                            ItemId = item.ItemId,
                            NameEn = item.NameEn,
                            NameAr = item.NameAr,
                            ItemType = item.ItemType,
                            BasePrice = item.BasePrice,
                            TaxRule = item.TaxRuleId != null && taxRuleLookup.ContainsKey(item.TaxRuleId.Value)
                                ? taxRuleLookup[item.TaxRuleId.Value]
                                : null,
                            Prices = pricesByItem.TryGetValue(item.ItemId, out var pr)
                                ? pr
                                : new List<ItemPricesDto>(),

                            // ===== NEW =====
                            ModifierGroups = itemModifierGroupDtos
                                .OrderBy(x => x.Sequence) // double-safety
                                .ToList()
                        });
                    }
                }

                categoryNodes.Add(catNode);
            }

            var branchNode = new BranchesDto
            {
                BranchId = branch.BranchId,
                Code = branch.Code,
                NameEn = branch.NameEn,
                NameAr = branch.NameAr,
                Address = branch.Address,
                Phone = branch.Phone,
                IsActive = branch.IsActive,
                ClientId = branch.ClientId,
                Categories = categoryNodes,
                TaxRules = taxRules.Select(t => new TaxRulesDto
                {
                    TaxRuleId = t.TaxRuleId,
                    NameAR = t.NameAR,
                    NameEN = t.NameEN,
                    Rate = t.Rate
                }).ToList(),
                PriceLists = priceLists.Select(p => new PriceListsDto
                {
                    PriceListId = p.PriceListId,
                    NameEn = p.NameEn,
                    NameAr = p.NameAr,
                    IsActive = p.IsActive,
                    ClientId = clientId
                }).ToList()
            };

            return new BranchCatalogResponse
            {
                Success = true,
                Message = "Branch catalog loaded successfully.",
                Menu = new MenuDto { Branches = new List<BranchesDto> { branchNode } }
            };
        }
    }
}
