using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Contracts.DTOS
{
    public sealed record PlaceOrderRequest(
     int BranchId,
     int PriceListId,
     string DeviceId,
     Guid? UserId,
     int? ShiftId,
     string? Notes,
     IReadOnlyList<PlaceOrderItemDto> Items
 );
    public sealed record PlaceOrderItemDto(
    int ItemId,
    int Qty,
    string? Notes,
    IReadOnlyList<PlaceOrderItemModifierGroupDto>? ModifierGroups
);

    public sealed record PlaceOrderItemModifierGroupDto(
        int ModifierGroupId,
        IReadOnlyList<int> Modifiers
    );

    public sealed record PlaceOrderResponse(
        bool Success,
        string Message,
        long? OrderId,
        string? OrderNumber
    );
}
