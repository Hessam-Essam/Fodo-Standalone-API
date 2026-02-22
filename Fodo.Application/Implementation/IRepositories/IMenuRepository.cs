using Fodo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Application.Implementation.IRepositories
{
    public interface IMenuRepository
    {
        Task<Branches?> GetBranchAsync(int branchId, CancellationToken ct);

        Task<List<Items>> GetItemsByIdsAsync(int clientId, IReadOnlyCollection<int> itemIds, CancellationToken ct);

        /// <summary>
        /// Returns itemId -> price for a single pricelistId (only overrides),
        /// service should fallback to Items.BasePrice when missing.
        /// </summary>
        Task<Dictionary<int, decimal>> GetItemPricesForPriceListAsync(IReadOnlyCollection<int> itemIds, int priceListId, CancellationToken ct);

        Task<List<TaxRules>> GetTaxRulesByClientAsync(int clientId, CancellationToken ct);

        Task<List<ItemsModifierGroup>> GetItemModifierGroupsByItemIdsAsync(int clientId, IReadOnlyCollection<int> itemIds, CancellationToken ct);

        Task<List<ModifiersGroup>> GetModifierGroupsByIdsAsync(int clientId, IReadOnlyCollection<int> groupIds, CancellationToken ct);

        Task<List<Modifiers>> GetModifiersByIdsAsync(int clientId, IReadOnlyCollection<int> modifierIds, CancellationToken ct);

        /// <summary>
        /// Returns modifierId -> price for a single pricelistId (only overrides),
        /// service should fallback to Modifiers.BasePrice when missing.
        /// </summary>
        Task<Dictionary<int, decimal>> GetModifierPricesForPriceListAsync(IReadOnlyCollection<int> modifierIds, int priceListId, CancellationToken ct);

        Task<Dictionary<int, decimal>> GetModifierPricesForPriceListAsync(
        int clientId,
        int priceListId,
        List<int> modifierIds,
        CancellationToken ct);
    }

}
