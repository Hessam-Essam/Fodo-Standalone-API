using Fodo.Application.Implementation.Interfaces;
using Fodo.Application.Implementation.IRepositories;
using Fodo.Contracts.DTOS;
using Fodo.Contracts.Responses;

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
            //if (branchId <= 0)
            //    return new BranchCatalogResponse { Success = false, Message = "branchId is required." };

            //var branch = await _repo.GetBranchAsync(branchId);
            //if (branch == null)
            //    return new BranchCatalogResponse { Success = false, Message = "Branch not found." };

            //var clientId = branch.ClientId;

            //var categoryBranches = await _repo.GetCategoryBranchesByBranchAsync(branchId);
            //var categoryIds = categoryBranches.Select(x => x.CategoryId).Distinct().ToList();

            //var categories = await _repo.GetCategoriesByIdsAsync(categoryIds);
            //categories = categories.Where(c => c.ClientId == clientId).ToList();

            //var items = await _repo.GetItemsByCategoryIdsAsync(categoryIds);
            //items = items.Where(i => i.ClientId == clientId).ToList();

            //var itemIds = items.Select(i => i.ItemId).Distinct().ToList();
            //var itemPricelists = await _repo.GetItemPricelistsByItemIdsAsync(itemIds);

            //var priceLists = await _repo.GetPriceListsByClientAsync(clientId);
            //var taxRules = await _repo.GetTaxRulesByClientAsync(clientId);

            //var branches = new List<BranchesDto>
            //{
            //    new BranchesDto
            //    {
            //        BranchId = branch.BranchId,
            //        NameEn = branch.NameEn,
            //        NameAr = branch.NameAr,
            //        IsActive = branch.IsActive,
            //        ClientId = branch.ClientId
            //    }
            //};

            //var categoriesDto = categories.Select(c => new CategoriesDto
            //{
            //    CategoryId = c.CategoryId,
            //    NameEn = c.NameEn,
            //    NameAr = c.NameAr,
            //    IsActive = c.IsActive,
            //    ClientId = c.ClientId
            //}).ToList();

            //var categoryBranchesDto = categoryBranches.Select(cb => new CategoryBranchesDto
            //{
            //    CategoryId = cb.CategoryId,
            //    BranchId = cb.BranchId
            //}).ToList();

            //var itemsDto = items.Select(i => new ItemsDto
            //{
            //    ItemId = i.ItemId,
            //    NameEn = i.NameEn,
            //    NameAr = i.NameAr,
            //    ItemType = i.ItemType,
            //    CategoryId = i.CategoryId,
            //    BasePrice = i.BasePrice,
            //    IsActive = i.IsActive,
            //    TaxRuleId = i.TaxRuleId,
            //    ClientId = i.ClientId
            //}).ToList();

            //var itemPricelistsDto = itemPricelists.Select(ip => new ItemPricelistsDto
            //{
            //    ItemId = ip.ItemId,
            //    PricelistId = ip.PricelistId,
            //    Price = ip.Price
            //}).ToList();

            //var priceListsDto = priceLists.Select(pl => new PriceListsDto
            //{
            //    PriceListId = pl.PriceListId,
            //    NameEn = pl.NameEn,
            //    NameAr = pl.NameAr,
            //    IsActive = pl.IsActive,
            //    ClientId = pl.ClientId
            //}).ToList();

            //var taxRulesDto = taxRules.Select(tr => new TaxRulesDto
            //{
            //    TaxRuleId = tr.TaxRuleId,
            //    NameAR = tr.NameAR,
            //    NameEN = tr.NameEN,
            //    Rate = tr.Rate,
            //    ClientId = tr.ClientId
            //}).ToList();

            //var branchesDto = branches;

            //return new BranchCatalogResponse
            //{
            //    Success = true,
            //    Message = "Catalog loaded successfully",
            //    Menu = new MenuDto
            //    {
            //        Categories = categoriesDto,
            //        CategoryBranches = categoryBranchesDto,
            //        Items = itemsDto,
            //        ItemPricelists = itemPricelistsDto,
            //        PriceLists = priceListsDto,
            //        TaxRules = taxRulesDto,
            //        Branches = branchesDto
            //    }
            //};
            if (branchId <= 0)
                return new BranchCatalogResponse
                {
                    Success = false,
                    Message = "branchId is required."
                };

            // ✅ Load branch
            var branch = await _repo.GetBranchAsync(branchId);

            if (branch == null)
                return new BranchCatalogResponse
                {
                    Success = false,
                    Message = "Branch not found."
                };

            int clientId = branch.ClientId;

            // ✅ CategoryBranches → Categories
            var categoryBranches = await _repo.GetCategoryBranchesByBranchAsync(branchId);

            var categoryIds = categoryBranches
                .Select(x => x.CategoryId)
                .Distinct()
                .ToList();

            var categories = await _repo.GetCategoriesByIdsAsync(categoryIds);

            // ✅ Items under categories
            var items = await _repo.GetItemsByCategoryIdsAsync(categoryIds);

            var itemIds = items.Select(i => i.ItemId).Distinct().ToList();

            // ✅ Prices per item
            var itemPricelists = await _repo.GetItemPricelistsByItemIdsAsync(itemIds);

            // ✅ Shared lookups
            var priceLists = await _repo.GetPriceListsByClientAsync(clientId);
            var taxRules = await _repo.GetTaxRulesByClientAsync(clientId);

            // ✅ Group for nesting
            var itemsByCategory = items.GroupBy(x => x.CategoryId)
                .ToDictionary(g => g.Key, g => g.ToList());
            var priceListLookup = priceLists.ToDictionary(
    x => x.PriceListId,
    x => new
    {
        x.NameEn,
        x.NameAr
    }
);
            var pricesByItem = itemPricelists
    .GroupBy(x => x.ItemId)
    .ToDictionary(
        g => g.Key,
        g => g.Select(p =>
        {
            // lookup name
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

            // ✅ Build nested categories
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
                var taxRuleLookup = taxRules.ToDictionary( x => x.TaxRuleId,x => new TaxRulesDto
                {
                    TaxRuleId = x.TaxRuleId,
                    NameEN = x.NameEN,
                    NameAR = x.NameAR,
                    Rate = x.Rate
                }
);
                if (itemsByCategory.TryGetValue(cat.CategoryId, out var catItems))
                {
                    foreach (var item in catItems)
                    {
                        catNode.Items.Add(new ItemsDto
                        {
                            ItemId = item.ItemId,
                            NameEn = item.NameEn,
                            NameAr = item.NameAr,
                            ItemType = item.ItemType,
                            BasePrice = item.BasePrice,
                            TaxRule = item.TaxRuleId != null &&taxRuleLookup.ContainsKey(item.TaxRuleId.Value)
                                      ? taxRuleLookup[item.TaxRuleId.Value]
                                      : null,
                            Prices = pricesByItem.TryGetValue(item.ItemId, out var pr)
                                ? pr
                                : new List<ItemPricesDto>()
                        });
                    }
                }

                categoryNodes.Add(catNode);
            }

            // ✅ Final branch node
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
                Menu = new MenuDto
                {
                    Branches = new List<BranchesDto> { branchNode }
                }
            };
        }
    }
}
