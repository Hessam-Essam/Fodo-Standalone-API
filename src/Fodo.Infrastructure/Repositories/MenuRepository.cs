using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Infrastructure.Repositories
{
    using Fodo.Application.Implementation.IRepositories;
    using Fodo.Domain.Entities;
    using Fodo.Infrastructure.Persistence;
    using Microsoft.EntityFrameworkCore;

    public sealed class MenuRepository : IMenuRepository
    {
        private readonly IdentityDbContext _db;

        public MenuRepository(IdentityDbContext db)
        {
            _db = db;
        }

        public Task<Branches?> GetBranchAsync(int branchId, CancellationToken ct)
        {
            if (branchId <= 0) throw new ArgumentOutOfRangeException(nameof(branchId));

            return _db.Branches
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.BranchId == branchId, ct);
        }

        public async Task<List<Items>> GetItemsByIdsAsync(int clientId, IReadOnlyCollection<int> itemIds, CancellationToken ct)
        {
            if (clientId <= 0) throw new ArgumentOutOfRangeException(nameof(clientId));
            if (itemIds == null) throw new ArgumentNullException(nameof(itemIds));
            if (itemIds.Count == 0) return new List<Items>();

            // If you want to block inactive items, add: && i.IsActive
            return await _db.Items
                .AsNoTracking()
                .Where(i => i.ClientId == clientId && itemIds.Contains(i.ItemId))
                .ToListAsync(ct);
        }

        public async Task<Dictionary<int, decimal>> GetItemPricesForPriceListAsync(
            IReadOnlyCollection<int> itemIds,
            int priceListId,
            CancellationToken ct)
        {
            if (itemIds == null) throw new ArgumentNullException(nameof(itemIds));
            if (itemIds.Count == 0) return new Dictionary<int, decimal>();
            if (priceListId <= 0) throw new ArgumentOutOfRangeException(nameof(priceListId));

            // Only returns overrides for the given pricelist
            return await _db.ItemPricelists
                .AsNoTracking()
                .Where(x => x.PricelistId == priceListId && itemIds.Contains(x.ItemId))
                .ToDictionaryAsync(x => x.ItemId, x => x.Price, ct);
        }

        public async Task<List<TaxRules>> GetTaxRulesByClientAsync(int clientId, CancellationToken ct)
        {
            if (clientId <= 0) throw new ArgumentOutOfRangeException(nameof(clientId));

            // If you have IsActive on TaxRules, filter it.
            return await _db.TaxRules
                .AsNoTracking()
                .Where(t => t.ClientId == clientId)
                .ToListAsync(ct);
        }

        public async Task<List<ItemsModifierGroup>> GetItemModifierGroupsByItemIdsAsync(
            int clientId,
            IReadOnlyCollection<int> itemIds,
            CancellationToken ct)
        {
            if (clientId <= 0) throw new ArgumentOutOfRangeException(nameof(clientId));
            if (itemIds == null) throw new ArgumentNullException(nameof(itemIds));
            if (itemIds.Count == 0) return new List<ItemsModifierGroup>();

            return await _db.ItemsModifierGroups
                .AsNoTracking()
                .Where(x => x.ClientId == clientId
                            && x.IsActive
                            && itemIds.Contains(x.ItemId))
                .OrderBy(x => x.ItemId)
                .ThenBy(x => x.SortOrder)
                .ToListAsync(ct);
        }

        public async Task<List<ModifiersGroup>> GetModifierGroupsByIdsAsync(
            int clientId,
            IReadOnlyCollection<int> groupIds,
            CancellationToken ct)
        {
            if (clientId <= 0) throw new ArgumentOutOfRangeException(nameof(clientId));
            if (groupIds == null) throw new ArgumentNullException(nameof(groupIds));
            if (groupIds.Count == 0) return new List<ModifiersGroup>();

            return await _db.ModifiersGroup
                .AsNoTracking()
                .Where(x => x.ClientId == clientId
                            && x.IsActive
                            && groupIds.Contains(x.Id))
                .ToListAsync(ct);
        }

        public async Task<List<Modifiers>> GetModifiersByIdsAsync(
            int clientId,
            IReadOnlyCollection<int> modifierIds,
            CancellationToken ct)
        {
            if (clientId <= 0) throw new ArgumentOutOfRangeException(nameof(clientId));
            if (modifierIds == null) throw new ArgumentNullException(nameof(modifierIds));
            if (modifierIds.Count == 0) return new List<Modifiers>();

            return await _db.Modifiers
                .AsNoTracking()
                .Where(x => x.ClientId == clientId
                            && x.IsActive
                            && modifierIds.Contains(x.Id))
                .ToListAsync(ct);
        }

        public async Task<Dictionary<int, decimal>> GetModifierPricesForPriceListAsync(
            IReadOnlyCollection<int> modifierIds,
            int priceListId,
            CancellationToken ct)
        {
            if (modifierIds == null) throw new ArgumentNullException(nameof(modifierIds));
            if (modifierIds.Count == 0) return new Dictionary<int, decimal>();
            if (priceListId <= 0) throw new ArgumentOutOfRangeException(nameof(priceListId));

            // Only returns overrides for the given pricelist
            return await _db.ModifiersPricelist
                .AsNoTracking()
                .Where(x => x.PriceListId == priceListId
                            && modifierIds.Contains(x.ModifierId))
                .ToDictionaryAsync(x => x.ModifierId, x => x.Price, ct);
        }

        public async Task<Dictionary<int, decimal>> GetModifierPricesForPriceListAsync(
        int clientId,
        int priceListId,
        List<int> modifierIds,
        CancellationToken ct)
        {
            if (modifierIds == null || modifierIds.Count == 0)
                return new Dictionary<int, decimal>();

            // Example table: ModifierPricelists(ModifierId, PricelistId, Price, ClientId)
            // Replace with your real table/columns if different.
            return await _db.ModifiersPricelist
                .AsNoTracking()
                .Where(x => x.PriceListId == priceListId
                         && modifierIds.Contains(x.ModifierId))
                .ToDictionaryAsync(x => x.ModifierId, x => x.Price, ct);
        }
    }

}
