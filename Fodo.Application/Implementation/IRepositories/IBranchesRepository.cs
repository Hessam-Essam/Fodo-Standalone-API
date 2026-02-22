using Fodo.Contracts.DTOS;
using Fodo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Application.Implementation.IRepositories
{
    public interface IBranchesRepository
    {
        Task<IReadOnlyList<BranchesDto>> GetByClientIdAsync(int clientId, CancellationToken ct);
        Task<Branches> GetBranchAsync(int branchId);
        Task<List<CategoryBranches>> GetCategoryBranchesByBranchAsync(int branchId);
        Task<List<Categories>> GetCategoriesByIdsAsync(List<int> categoryIds);
        Task<List<Items>> GetItemsByCategoryIdsAsync(List<int> categoryIds);
        Task<List<ItemPricelists>> GetItemPricelistsByItemIdsAsync(List<int> itemIds);
        Task<List<PriceLists>> GetPriceListsByClientAsync(int clientId);
        Task<List<TaxRules>> GetTaxRulesByClientAsync(int clientId);
        Task<List<ItemsModifierGroup>> GetItemModifierGroupsByItemIdsAsync(int clientId,IReadOnlyCollection<int> itemIds,CancellationToken ct = default);
        Task<List<ModifiersGroup>> GetModifierGroupsByIdsAsync(int clientId, IReadOnlyCollection<int> groupIds, CancellationToken ct = default);
        Task<List<Modifiers>> GetModifiersByGroupIdsAsync(int clientId, IReadOnlyCollection<int> groupIds, CancellationToken ct = default);
        Task<List<ModifiersPricelist>> GetModifierPricelistsByModifierIdsAsync(IReadOnlyCollection<int> modifierIds, CancellationToken ct = default);
    }
}
