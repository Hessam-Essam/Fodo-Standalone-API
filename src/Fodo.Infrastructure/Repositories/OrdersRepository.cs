using Fodo.Application.Features.Orders;
using Fodo.Application.Implementation.IRepositories;
using Fodo.Domain.Entities;
using Fodo.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Fodo.Infrastructure.Repositories
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly IdentityDbContext _db;
        public OrdersRepository(IdentityDbContext db) => _db = db;

        public async Task<string> GenerateOrderNumberAsync(int branchId, CancellationToken ct)
        {
            var date = DateTime.UtcNow.Date;
            var prefix = $"BR{branchId}-{date:yyyyMMdd}-";

            var count = await _db.Orders.AsNoTracking()
                .CountAsync(o => o.BranchId == branchId
                              && o.CreatedAt >= date
                              && o.CreatedAt < date.AddDays(1), ct);

            return prefix + (count + 1).ToString("D4");
        }

        public async Task<long> CreateOrderAsync(Orders order, CancellationToken ct)
        {
            _db.Orders.Add(order);
            await _db.SaveChangesAsync(ct); // will insert header + items + modifiers (via navigation graph)
            return order.OrderId;
        }

        public async Task<(List<Orders> Orders, int TotalCount)> GetOrdersPagedAsync(GetOrdersQuery query, CancellationToken ct)
        {
            var q = _db.Orders.AsNoTracking()
                .Where(o => o.BranchId == query.BranchId);

            if (query.ShiftId.HasValue)
                q = q.Where(o => o.ShiftId == query.ShiftId.Value);

            if (!string.IsNullOrWhiteSpace(query.Status))
                q = q.Where(o => o.Status == query.Status);

            if (query.FromUtc.HasValue)
                q = q.Where(o => o.CreatedAt >= query.FromUtc.Value);

            if (query.ToUtc.HasValue)
                q = q.Where(o => o.CreatedAt <= query.ToUtc.Value);

            var totalCount = await q.CountAsync(ct);

            // Include only after filtering (best perf)
            var page = query.Page <= 0 ? 1 : query.Page;

            // Dynamic sizing with server-side safety clamp
            const int minSize = 5;
            const int maxSize = 100;
            var size = query.PageSize <= 0 ? 20 : query.PageSize;
            size = Math.Clamp(size, minSize, maxSize);

            var orders = await q
                .OrderByDescending(o => o.CreatedAt)
                .Include(o => o.OrderItems)
                    .ThenInclude(i => i.Modifiers)
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync(ct);

            return (orders, totalCount);
        }

        public Task<Orders?> GetByIdAsync(long orderId, CancellationToken ct)
            => _db.Orders.FirstOrDefaultAsync(o => o.OrderId == orderId, ct);

        public void Update(Orders order) => _db.Orders.Update(order);

        public Task CreateOrderItemsAsync(List<OrderItems> items, CancellationToken ct)
        {
            _db.OrderItems.AddRange(items);
            return Task.CompletedTask;
        }

        public Task CreateOrderItemModifiersAsync(List<OrderItemModifier> modifiers, CancellationToken ct)
        {
            if (modifiers.Count > 0)
                _db.OrderItemModifier.AddRange(modifiers);

            return Task.CompletedTask;
        }

        public void AddOrder(Orders order) => _db.Orders.Add(order);

        public void AddOrderItems(IEnumerable<OrderItems> items) => _db.OrderItems.AddRange(items);

        public void AddOrderItemModifiers(IEnumerable<OrderItemModifier> modifiers)
        {
            var list = modifiers?.ToList() ?? new List<OrderItemModifier>();
            if (list.Count > 0)
                _db.OrderItemModifier.AddRange(list);
        }

        public async Task<List<Orders>> GetOrdersByIdsAsync(
        int branchId,
        List<long> orderIds,
        DateTime? fromUtc,
        DateTime? toUtc,
        int? shiftId,
        CancellationToken ct)
        {
            if (branchId <= 0)
                return new List<Orders>();

            if (orderIds is null || orderIds.Count == 0)
                return new List<Orders>();

            // IMPORTANT: avoid huge IN() lists; you can chunk if needed
            var q = _db.Orders
                .AsNoTracking()
                .Where(o => o.BranchId == branchId)
                .Where(o => orderIds.Contains(o.OrderId));

            if (shiftId.HasValue)
                q = q.Where(o => o.ShiftId == shiftId.Value);

            if (fromUtc.HasValue)
                q = q.Where(o => o.CreatedAt >= fromUtc.Value);

            if (toUtc.HasValue)
                q = q.Where(o => o.CreatedAt <= toUtc.Value);

            // If you need items/modifiers in same response, include them here:
            // q = q.Include(o => o.OrderItems).ThenInclude(i => i.Modifiers);

            return await q
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync(ct);
        }

        public async Task<List<long>> GetOrderIdsByShiftAsync(
    int branchId,
    int shiftId,
    CancellationToken ct)
        {
            return await _db.Orders
                .AsNoTracking()
                .Where(o => o.BranchId == branchId
                            && o.ShiftId == shiftId)
                .Select(o => o.OrderId)
                .Distinct()
                .ToListAsync(ct);
        }

        public async Task<DateTime?> GetLastOrderCreatedAtByShiftAsync(int branchId, int shiftId, CancellationToken ct)
        {
            return await _db.Orders
                .AsNoTracking()
                .Where(o => o.BranchId == branchId && o.ShiftId == shiftId)
                .MaxAsync(o => (DateTime?)o.CreatedAt, ct);
        }
    }
}
