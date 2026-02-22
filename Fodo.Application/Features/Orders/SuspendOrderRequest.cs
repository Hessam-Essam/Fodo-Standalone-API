using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Application.Features.Orders
{
    public sealed record SuspendOrderRequest(
    long OrderId,
    int BranchId,
    string DeviceId,
    Guid? UserId,
    string? Notes
);

    public sealed record SuspendOrderResponse(
        bool Success,
        string Message
    );
}
