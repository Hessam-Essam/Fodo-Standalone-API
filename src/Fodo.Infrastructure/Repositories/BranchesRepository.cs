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

        public async Task<List<ItemsModifierGroup>> GetItemModifierGroupsByItemIdsAsync(int clientId,IReadOnlyCollection<int> itemIds,CancellationToken ct = default)
        {
            if (clientId <= 0) 
                throw new ArgumentOutOfRangeException(nameof(clientId));
            if (itemIds == null) 
                throw new ArgumentNullException(nameof(itemIds));
            if (itemIds.Count == 0) 
                return new List<ItemsModifierGroup>();

            return await _db.ItemsModifierGroups
                .AsNoTracking()
                .Where(x => x.ClientId == clientId
                            && x.IsActive
                            && itemIds.Contains(x.ItemId))
                .OrderBy(x => x.ItemId)
                .ThenBy(x => x.SortOrder)
                .ToListAsync(ct);
        }

        public async Task<List<ModifiersGroup>> GetModifierGroupsByIdsAsync(int clientId, IReadOnlyCollection<int> groupIds, CancellationToken ct = default)
        {
            if (clientId <= 0) 
                throw new ArgumentOutOfRangeException(nameof(clientId));
            if (groupIds == null) 
                throw new ArgumentNullException(nameof(groupIds));
            if (groupIds.Count == 0) 
                return new List<ModifiersGroup>();

            return await _db.ModifiersGroup
                .AsNoTracking()
                .Where(x => x.ClientId == clientId
                            && x.IsActive
                            && groupIds.Contains(x.Id))
                .OrderBy(x => x.Id)
                .ToListAsync(ct);
        }

        public async Task<List<Modifiers>> GetModifiersByGroupIdsAsync(int clientId,IReadOnlyCollection<int> groupIds, CancellationToken ct = default)
        {
            if (clientId <= 0) 
                throw new ArgumentOutOfRangeException(nameof(clientId));
            if (groupIds == null) 
                throw new ArgumentNullException(nameof(groupIds));
            if (groupIds.Count == 0) 
                return new List<Modifiers>();

            return await _db.Modifiers
                .AsNoTracking()
                .Where(x => x.ClientId == clientId
                            && x.IsActive
                            && groupIds.Contains(x.ModifierGroupId))
                .OrderBy(x => x.ModifierGroupId)
                .ThenBy(x => x.SortOrder) // sequence inside the group
                .ThenBy(x => x.Id)
                .ToListAsync(ct);
        }

        public async Task<List<ModifiersPricelist>> GetModifierPricelistsByModifierIdsAsync(IReadOnlyCollection<int> modifierIds, CancellationToken ct = default)
        {
            if (modifierIds == null) 
                throw new ArgumentNullException(nameof(modifierIds));
            if (modifierIds.Count == 0) 
                return new List<ModifiersPricelist>();

            // Note: ModifierPricelists table doesn't have ClientId (by design).
            // We rely on modifierIds already being filtered by client in previous queries.
            return await _db.ModifiersPricelist
                .AsNoTracking()
                .Where(x => modifierIds.Contains(x.ModifierId))
                .OrderBy(x => x.ModifierId)
                .ThenBy(x => x.PriceListId)
                .ToListAsync(ct);
        }
    }

}
