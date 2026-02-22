using Fodo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Application.Features.Orders
{
    public sealed record OrdersListResponse(
    bool Success,
    string Message,
    IReadOnlyList<OrderListItemDto> Orders
);

    public sealed record OrderListItemDto(
        long OrderId,
        string OrderNumber,
        int BranchId,
        Guid? UserId,
        string? UserNameEn,
        string? UserNameAr,
        int? ShiftId,
        string Status,
        decimal SubTotal,
        decimal TaxAmount,
        decimal TotalAmount,
        string Note,
        DateTime CreatedAt,
        IReadOnlyList<OrderItemDto>? Items
    );

    public sealed record OrderItemDto(
        long OrderItemId,
        int ItemId,
        string? NameEn,
        string? NameAr,
        int Qty,
        string Note,
        decimal Price, // unit price in your schema
        IReadOnlyList<OrderItemModifierDto> OrderItemModifier
    );

    public sealed record OrderItemModifierDto(
        long OrderItemModifierId,
        int ModifierGroupId,
        int ModifierId,
        string? NameEn,
    string? NameAr,
    decimal BasePrice,
    decimal UnitPrice
    );
    public sealed record Modifiers(
        string NameEn,
        string? NameAr,
        decimal BasePrice
    );
    public sealed record GetOrdersQuery(
    int BranchId,
    int? ShiftId,
    string? Status,
    DateTime? FromUtc,
    DateTime? ToUtc,
    int Page = 1,
    int PageSize = 10
);

}
