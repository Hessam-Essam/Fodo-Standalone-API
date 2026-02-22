using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Application.Features.Orders
{
    public sealed record SuspendOrderCreateRequest(
    int BranchId,
    int PriceListId,
    string DeviceId,
    Guid? UserId,
    int? ShiftId,
    string? Notes,
    IReadOnlyList<SuspendOrderItemRequest> Items
);

    public sealed record SuspendOrderItemRequest(
        int ItemId,
        int Qty,
        string? Notes,
        IReadOnlyList<SuspendOrderModifierGroupRequest>? ModifierGroups
    );

    public sealed record SuspendOrderModifierGroupRequest(
        int ModifierGroupId,
        IReadOnlyList<int> Modifiers
    );

    public sealed record SuspendOrderCreateResponse(
    bool Success,
    string Message,
    long? OrderId,
    string? OrderNumber
);

}
