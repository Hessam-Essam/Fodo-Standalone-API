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
            if (branchId <= 0)
                return new BranchCatalogResponse { Success = false, Message = "branchId is required." };

            var branch = await _repo.GetBranchAsync(branchId);
            if (branch == null)
                return new BranchCatalogResponse { Success = false, Message = "Branch not found." };

            var clientId = branch.ClientId; // IMPORTANT: comes from Branches.Client_id mapped to ClientId

            var categoryBranches = await _repo.GetCategoryBranchesByBranchAsync(branchId);
            var categoryIds = categoryBranches.Select(x => x.CategoryId).Distinct().ToList();

            var categories = await _repo.GetCategoriesByIdsAsync(categoryIds);
            // (optional) only categories for same client:
            categories = categories.Where(c => c.ClientId == clientId).ToList();

            var items = await _repo.GetItemsByCategoryIdsAsync(categoryIds);
            items = items.Where(i => i.ClientId == clientId).ToList();

            var itemIds = items.Select(i => i.ItemId).Distinct().ToList();
            var itemPricelists = await _repo.GetItemPricelistsByItemIdsAsync(itemIds);

            var priceLists = await _repo.GetPriceListsByClientAsync(clientId);
            var taxRules = await _repo.GetTaxRulesByClientAsync(clientId);

            return new BranchCatalogResponse
            {
                Success = true,
                Message = "Branch catalog loaded successfully.",

                Branch = new BranchesDto
                {
                    BranchId = branch.BranchId,
                    Code = branch.Code,
                    NameEn = branch.NameEn,
                    NameAr = branch.NameAr,
                    Address = branch.Address,
                    Phone = branch.Phone,
                    IsActive = branch.IsActive,
                    ClientId = branch.ClientId
                },

                Categories = categories.Select(c => new CategoriesDto
                {
                    CategoryId = c.CategoryId,
                    NameEn = c.NameEn,
                    NameAr = c.NameAr,
                    IsActive = c.IsActive,
                    ClientId = c.ClientId
                }).ToList(),

                CategoryBranches = categoryBranches.Select(cb => new CategoryBranchesDto
                {
                    CategoryId = cb.CategoryId,
                    BranchId = cb.BranchId
                }).ToList(),

                Items = items.Select(i => new ItemsDto
                {
                    ItemId = i.ItemId,
                    NameEn = i.NameEn,
                    NameAr = i.NameAr,
                    ItemType = i.ItemType,
                    CategoryId = i.CategoryId,
                    BasePrice = i.BasePrice,
                    IsActive = i.IsActive,
                    TaxRuleId = i.TaxRuleId,
                    ClientId = i.ClientId
                }).ToList(),

                ItemPricelists = itemPricelists.Select(ip => new ItemPricelistsDto
                {
                    ItemId = ip.ItemId,
                    PricelistId = ip.PricelistId,
                    Price = ip.Price
                }).ToList(),

                PriceLists = priceLists.Select(p => new PriceListsDto
                {
                    PriceListId = p.PriceListId,
                    NameEn = p.NameEn,
                    NameAr = p.NameAr,
                    IsActive = p.IsActive,
                    ClientId = p.ClientId
                }).ToList(),

                TaxRules = taxRules.Select(t => new TaxRulesDto
                {
                    TaxRuleId = t.TaxRuleId,
                    NameAR = t.NameAR,
                    NameEN = t.NameEN,
                    Rate = t.Rate,
                    ClientId = t.ClientId
                }).ToList()
            };
        }
    }
}
