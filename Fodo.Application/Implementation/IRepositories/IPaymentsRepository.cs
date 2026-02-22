using Fodo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Application.Implementation.IRepositories
{
    public interface IPaymentsRepository
    {
        Task<List<int>> GetVisaPaymentMethodIdsAsync(int clientId, CancellationToken ct);

        Task<Dictionary<int, (string MethodName, string MethodType)>> GetPaymentMethodInfoAsync(
            int clientId,
            List<int> methodIds,
            CancellationToken ct);

        Task<List<long>> GetOrderIdsWithPaymentMethodsAsync(
            List<int> paymentMethodIds,
            CancellationToken ct);

        Task<Dictionary<long, List<PaymentRow>>> GetPaymentsForOrdersAsync(
            List<long> orderIds,
            List<int> paymentMethodIds,
            CancellationToken ct);

        Task<List<int>> GetCashPaymentMethodIdsAsync(int clientId, CancellationToken ct);

        Task<decimal> SumPaymentsByOrderIdsAndMethodsAsync(
        List<long> orderIds,
        List<int> paymentMethodIds,
        CancellationToken ct);

     

    }
    public sealed record PaymentRow(
    long PaymentId,
    long OrderId,
    int PaymentMethodId,
    string? Method,
    decimal Amount,
    DateTime CreatedAt,
    string DeviceId
);
}
