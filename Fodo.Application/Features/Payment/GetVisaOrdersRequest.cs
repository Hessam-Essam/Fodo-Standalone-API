using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Application.Features.Payment
{
    public sealed record GetVisaOrdersRequest(
    int BranchId,
    string DeviceId,
    DateTime? FromUtc = null,
    DateTime? ToUtc = null,
    int? ShiftId = null
);
    public sealed record VisaOrdersResponse(
    bool Success,
    string Message,
    IReadOnlyList<VisaOrderDto> Orders
);

    public sealed record VisaOrderDto(
        long OrderId,
        string OrderNumber,
        int BranchId,
        int? ShiftId,
        Guid? UserId,
        string Status,
        decimal SubTotal,
        decimal TaxAmount,
        decimal TotalAmount,
        DateTime CreatedAt,
        decimal VisaPaidAmount,
        IReadOnlyList<VisaPaymentDto> VisaPayments
    );

    public sealed record VisaPaymentDto(
        long PaymentId,
        int PaymentMethodId,
        string MethodName,
        string MethodType,
        decimal Amount,
        DateTime CreatedAt,
        string DeviceId
    );
}
