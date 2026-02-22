using Fodo.Application.Implementation.IRepositories;
using Fodo.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Fodo.Infrastructure.Repositories
{
    public class PaymentsRepository : IPaymentsRepository
    {
        private readonly IdentityDbContext _db;

        public PaymentsRepository(IdentityDbContext db)
        {
            _db = db;
        }

        public async Task<List<int>> GetVisaPaymentMethodIdsAsync(int clientId, CancellationToken ct)
        {
            // VISA detection: method_type == "Visa" OR method_name contains "visa"
            return await _db.Payment_Methods
                .AsNoTracking()
                .Where(m => m.ClientId == clientId
                            && m.IsActive
                            && (m.MethodType == "Visa"
                                || EF.Functions.Like(m.MethodName.ToLower(), "%visa%")))
                .Select(m => m.PaymentMethodId)
                .Distinct()
                .ToListAsync(ct);
        }

        public async Task<Dictionary<int, (string MethodName, string MethodType)>> GetPaymentMethodInfoAsync(
            int clientId,
            List<int> methodIds,
            CancellationToken ct)
        {
            if (methodIds == null || methodIds.Count == 0)
                return new Dictionary<int, (string, string)>();

            return await _db.Payment_Methods
                .AsNoTracking()
                .Where(m => m.ClientId == clientId && methodIds.Contains(m.PaymentMethodId))
                .Select(m => new { m.PaymentMethodId, m.MethodName, m.MethodType })
                .ToDictionaryAsync(
                    x => x.PaymentMethodId,
                    x => (x.MethodName, x.MethodType),
                    ct);
        }

        public async Task<List<long>> GetOrderIdsWithPaymentMethodsAsync(
            List<int> paymentMethodIds,
            CancellationToken ct)
        {
            if (paymentMethodIds == null || paymentMethodIds.Count == 0)
                return new List<long>();

            return await _db.Payments
                .AsNoTracking()
                .Where(p => p.PaymentMethodId.HasValue && paymentMethodIds.Contains(p.PaymentMethodId.Value))
                .Select(p => p.OrderId)
                .Distinct()
                .ToListAsync(ct);
        }

        public async Task<Dictionary<long, List<PaymentRow>>> GetPaymentsForOrdersAsync(
            List<long> orderIds,
            List<int> paymentMethodIds,
            CancellationToken ct)
        {
            if (orderIds == null || orderIds.Count == 0 || paymentMethodIds == null || paymentMethodIds.Count == 0)
                return new Dictionary<long, List<PaymentRow>>();

            var rows = await _db.Payments
                .AsNoTracking()
                .Where(p => orderIds.Contains(p.OrderId)
                            && p.PaymentMethodId.HasValue
                            && paymentMethodIds.Contains(p.PaymentMethodId.Value))
                .OrderBy(p => p.CreatedAt)
                .Select(p => new PaymentRow(
                    PaymentId: p.PaymentId,
                    OrderId: p.OrderId,
                    PaymentMethodId: p.PaymentMethodId!.Value,
                    Method: p.Method,
                    Amount: p.Amount,
                    CreatedAt: p.CreatedAt,
                    DeviceId: p.DeviceId
                ))
                .ToListAsync(ct);

            return rows
                .GroupBy(x => x.OrderId)
                .ToDictionary(g => g.Key, g => g.ToList());
        }

        public async Task<decimal> SumPaymentsByOrderIdsAndMethodsAsync(
    List<long> orderIds,
    List<int> paymentMethodIds,
    CancellationToken ct)
        {
            if (orderIds == null || orderIds.Count == 0)
                return 0m;

            if (paymentMethodIds == null || paymentMethodIds.Count == 0)
                return 0m;

            return await _db.Payments
                .AsNoTracking()
                .Where(p =>
                    orderIds.Contains(p.OrderId) &&
                    p.PaymentMethodId.HasValue &&
                    paymentMethodIds.Contains(p.PaymentMethodId.Value))
                .SumAsync(p => (decimal?)p.Amount, ct) ?? 0m;
        }

        public async Task<List<int>> GetCashPaymentMethodIdsAsync(int clientId, CancellationToken ct)
        {
            return await _db.Payment_Methods
                .AsNoTracking()
                .Where(m => m.ClientId == clientId && m.IsActive
                            && (m.MethodType == "Cash"
                                || EF.Functions.Like(m.MethodName.ToLower(), "%cash%")
                                || EF.Functions.Like(m.MethodName.ToLower(), "%نقد%")))
                .Select(m => m.PaymentMethodId)
                .Distinct()
                .ToListAsync(ct);
        }
    }
}
