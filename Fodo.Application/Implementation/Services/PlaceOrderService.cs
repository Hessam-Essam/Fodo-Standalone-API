using Fodo.Application.Implementation.Interfaces;
using Fodo.Application.Implementation.IRepositories;
using Fodo.Contracts.DTOS;
using Fodo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Application.Implementation.Services
{
    public sealed class PlaceOrderService : IPlaceOrderService
    {
        private readonly IMenuRepository _menu;
        private readonly IOrdersRepository _orders;
        private readonly IUnitOfWork _uow;

        public PlaceOrderService(IMenuRepository menu, IOrdersRepository orders, IUnitOfWork uow)
        {
            _menu = menu;
            _orders = orders;
            _uow = uow;
        }

        public async Task<PlaceOrderResponse> PlaceOrderAsync(PlaceOrderRequest request, CancellationToken ct)
        {
            if (request.BranchId <= 0) return Fail("branchId is required.");
            if (request.PriceListId <= 0) return Fail("priceListId is required.");
            if (string.IsNullOrWhiteSpace(request.DeviceId)) return Fail("deviceId is required.");
            if (request.Items == null || request.Items.Count == 0) return Fail("items is required.");
            if (request.Items.Any(x => x.Qty <= 0)) return Fail("qty must be > 0.");

            var branch = await _menu.GetBranchAsync(request.BranchId, ct);
            if (branch == null) return Fail("Branch not found.");
            var clientId = branch.ClientId;

            var itemIds = request.Items.Select(i => i.ItemId).Distinct().ToList();

            var groupIds = request.Items
                .SelectMany(i => i.ModifierGroups ?? Array.Empty<PlaceOrderItemModifierGroupDto>())
                .Select(g => g.ModifierGroupId)
                .Distinct().ToList();

            var modifierIds = request.Items
                .SelectMany(i => i.ModifierGroups ?? Array.Empty<PlaceOrderItemModifierGroupDto>())
                .SelectMany(g => g.Modifiers ?? Array.Empty<int>())
                .Distinct().ToList();

            // Bulk reads
            var items = await _menu.GetItemsByIdsAsync(clientId, itemIds, ct);
            if (items.Count != itemIds.Count) return Fail("One or more items not found.");
            var itemById = items.ToDictionary(x => x.ItemId);

            var itemPrices = await _menu.GetItemPricesForPriceListAsync(itemIds, request.PriceListId, ct);
            var taxRules = await _menu.GetTaxRulesByClientAsync(clientId, ct);
            var taxById = taxRules.ToDictionary(x => x.TaxRuleId);

            var itemGroupLinks = await _menu.GetItemModifierGroupsByItemIdsAsync(clientId, itemIds, ct);
            var allowedGroupsByItem = itemGroupLinks
                .Where(x => x.IsActive)
                .GroupBy(x => x.ItemId)
                .ToDictionary(g => g.Key, g => g.Select(x => x.ModifierGroupId).ToHashSet());

            var groups = groupIds.Count == 0 ? new List<ModifiersGroup>() : await _menu.GetModifierGroupsByIdsAsync(clientId, groupIds, ct);
            var groupById = groups.Where(g => g.IsActive).ToDictionary(g => g.Id);

            var modifiers = modifierIds.Count == 0 ? new List<Modifiers>() : await _menu.GetModifiersByIdsAsync(clientId, modifierIds, ct);
            var modifierById = modifiers.Where(m => m.IsActive).ToDictionary(m => m.Id);

            var modifierPrices = modifierIds.Count == 0
                ? new Dictionary<int, decimal>()
                : await _menu.GetModifierPricesForPriceListAsync(modifierIds, request.PriceListId, ct);

            decimal subTotal = 0m;
            decimal taxTotal = 0m;

            var order = new Orders
            {
                BranchId = request.BranchId,
                UserId = request.UserId,
                ShiftId = request.ShiftId,
                Status = "Processing",
                CreatedAt = DateTime.UtcNow,
                RowGuid = Guid.NewGuid(),
                DeviceId = request.DeviceId,
                IsSynced = false,

                // No sequence: safe unique value without DB read
                OrderNumber = $"M-{Guid.NewGuid():N}".ToUpperInvariant()
            };

            foreach (var reqLine in request.Items)
            {
                var item = itemById[reqLine.ItemId];
                var baseUnit = itemPrices.TryGetValue(item.ItemId, out var ip) ? ip : item.BasePrice;

                decimal taxRate = 0m;
                if (item.TaxRuleId.HasValue && taxById.TryGetValue(item.TaxRuleId.Value, out var tr))
                    taxRate = tr.Rate;

                allowedGroupsByItem.TryGetValue(item.ItemId, out var allowedGroups);
                allowedGroups ??= new HashSet<int>();

                var submittedGroups = reqLine.ModifierGroups ?? Array.Empty<PlaceOrderItemModifierGroupDto>();
                if (submittedGroups.GroupBy(g => g.ModifierGroupId).Any(g => g.Count() > 1))
                    return Fail($"Duplicate modifierGroupId for item {item.ItemId}.");

                decimal modifiersUnitTotal = 0m;

                var orderItem = new OrderItems
                {
                    ItemId = item.ItemId,
                    Qty = reqLine.Qty
                };

                foreach (var gReq in submittedGroups)
                {
                    if (!allowedGroups.Contains(gReq.ModifierGroupId))
                        return Fail($"Modifier group {gReq.ModifierGroupId} not allowed for item {item.NameEn}.");

                    if (!groupById.TryGetValue(gReq.ModifierGroupId, out var grp))
                        return Fail($"Modifier group {gReq.ModifierGroupId} not found/inactive.");

                    var selected = (gReq.Modifiers ?? Array.Empty<int>()).Distinct().ToList();

                    if (grp.IsRequired && selected.Count == 0)
                        return Fail($"Modifier group {grp.NameEn} is required for item {item.NameEn}.");
                    if (selected.Count < grp.MinSelect)
                        return Fail($"Modifier group {grp.NameEn} requires at least {grp.MinSelect}.");
                    if (selected.Count > grp.MaxSelect)
                        return Fail($"Modifier group {grp.NameEn} allows at most {grp.MaxSelect}.");

                    foreach (var modId in selected)
                    {
                        if (!modifierById.TryGetValue(modId, out var mod))
                            return Fail($"Modifier {modId} not found/inactive.");
                        if (mod.ModifierGroupId != grp.Id)
                            return Fail($"Modifier {mod.NameEn} does not belong to group {grp.NameEn}.");

                        var modUnit = modifierPrices.TryGetValue(mod.Id, out var mp) ? mp : mod.BasePrice;
                        modifiersUnitTotal += modUnit;

                        orderItem.Modifiers.Add(new OrderItemModifier
                        {
                            ModifierGroupId = grp.Id,
                            ModifierId = mod.Id,
                            UnitPrice = Round(modUnit)
                        });
                    }
                }

                // Store UNIT price (base + modifiers) in OrderItems.Price
                var finalUnit = baseUnit + modifiersUnitTotal;
                orderItem.Price = Round(finalUnit);

                var lineSubTotal = finalUnit * orderItem.Qty;
                var lineTax = Round(lineSubTotal * taxRate);

                subTotal += lineSubTotal;
                taxTotal += lineTax;

                order.OrderItems.Add(orderItem);
            }

            order.SubTotal = Round(subTotal);
            order.TaxAmount = Round(taxTotal);
            order.TotalAmount = Round(order.SubTotal + order.TaxAmount);

            await _uow.BeginAsync(ct);
            try
            {
                var orderId = await _orders.CreateOrderAsync(order, ct);
                await _uow.CommitAsync(ct);

                return new PlaceOrderResponse(true, "Order submitted successfully.", orderId, order.OrderNumber);
            }
            catch
            {
                await _uow.RollbackAsync(ct);
                throw;
            }

            static PlaceOrderResponse Fail(string msg) => new(false, msg, null, null);
            static decimal Round(decimal v) => Math.Round(v, 2, MidpointRounding.AwayFromZero);
        }
    }

}
