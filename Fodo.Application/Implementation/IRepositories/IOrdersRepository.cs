using Fodo.Application.Features.Orders;
using Fodo.Domain.Entities;

namespace Fodo.Application.Implementation.IRepositories
{
    public interface IOrdersRepository
    {
        Task<string> GenerateOrderNumberAsync(int branchId, CancellationToken ct);
        Task<long> CreateOrderAsync(Orders order, CancellationToken ct);
        void AddOrder(Orders order);
        void AddOrderItems(IEnumerable<OrderItems> items);
        void AddOrderItemModifiers(IEnumerable<OrderItemModifier> modifiers);
        Task<(List<Orders> Orders, int TotalCount)> GetOrdersPagedAsync(GetOrdersQuery query, CancellationToken ct);
        Task<Orders?> GetByIdAsync(long orderId, CancellationToken ct);
        Task<List<Orders>> GetOrdersByIdsAsync(
        int branchId,
        List<long> orderIds,
        DateTime? fromUtc,
        DateTime? toUtc,
        int? shiftId,
        CancellationToken ct);

        Task<List<long>> GetOrderIdsByShiftAsync(int branchId, int shiftId, CancellationToken ct);
        Task<DateTime?> GetLastOrderCreatedAtByShiftAsync(int branchId, int shiftId, CancellationToken ct);
        void Update(Orders order);
        


    }
}
