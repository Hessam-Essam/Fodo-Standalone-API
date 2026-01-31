using Fodo.Application.Implementation.IRepositories;
using Fodo.Contracts.DTOS;
using Fodo.Domain.Entities;
using Fodo.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Fodo.Infrastructure.Repositories
{
    public class BranchesRepository : IBranchesRepository
    {
        private readonly IdentityDbContext _db;

        public BranchesRepository(IdentityDbContext db) => _db = db;

        public async Task<IReadOnlyList<BranchesDto>> GetByClientIdAsync(int clientId, CancellationToken ct)
        {
            return await _db.Branches
                .Where(b => b.ClientId == clientId)
                .OrderBy(b => b.BranchId)
                .Select(b => new BranchesDto
                {
                    BranchId = b.BranchId,
                    Code = b.Code,
                    NameEn = b.NameEn,
                    NameAr = b.NameAr,
                    Address = b.Address,
                    Phone = b.Phone,
                    IsActive = b.IsActive,
                    ClientId = b.ClientId
                })
                .ToListAsync(ct);
        }

        public Task<Branches> GetBranchAsync(int branchId)
        => _db.Branches.AsNoTracking().FirstOrDefaultAsync(x => x.BranchId == branchId);

        public Task<List<CategoryBranches>> GetCategoryBranchesByBranchAsync(int branchId)
            => _db.CategoryBranches
                .Where(x => x.BranchId == branchId)
                .AsNoTracking()
                .ToListAsync();

        public Task<List<Categories>> GetCategoriesByIdsAsync(List<int> categoryIds)
        {
            if (categoryIds == null || categoryIds.Count == 0)
                return Task.FromResult(new List<Categories>());

            return _db.Categories
                .Where(c => categoryIds.Contains(c.CategoryId))
                .AsNoTracking()
                .ToListAsync();
        }

        public Task<List<Items>> GetItemsByCategoryIdsAsync(List<int> categoryIds)
        {
            if (categoryIds == null || categoryIds.Count == 0)
                return Task.FromResult(new List<Items>());

            return _db.Items
                .Where(i => categoryIds.Contains(i.CategoryId))
                .AsNoTracking()
                .ToListAsync();
        }

        public Task<List<ItemPricelists>> GetItemPricelistsByItemIdsAsync(List<int> itemIds)
        {
            if (itemIds == null || itemIds.Count == 0)
                return Task.FromResult(new List<ItemPricelists>());

            return _db.ItemPricelists
                .Where(ip => itemIds.Contains(ip.ItemId))
                .AsNoTracking()
                .ToListAsync();
        }

        public Task<List<PriceLists>> GetPriceListsByClientAsync(int clientId)
            => _db.PriceLists.Where(x => x.ClientId == clientId).AsNoTracking().ToListAsync();

        public Task<List<TaxRules>> GetTaxRulesByClientAsync(int clientId)
            => _db.TaxRules.Where(x => x.ClientId == clientId).AsNoTracking().ToListAsync();
    }

}
