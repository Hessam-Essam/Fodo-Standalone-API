using Fodo.Application.Features.Orders;
using Fodo.Application.Features.Pagination;
using Fodo.Application.Features.Payment;
using Fodo.Application.Implementation.Interfaces;
using Fodo.Application.Implementation.IRepositories;
using Fodo.Domain.Entities;

namespace Fodo.Application.Implementation.Services
{
    public sealed class OrdersService : IOrdersService
    {
        private readonly IOrdersRepository _repo;
        private readonly IPaymentsRepository _payRepo;
        private readonly IMenuRepository _menuRepo;
        private readonly IUserRepository _userRepo;
        private readonly IUnitOfWork _uow;
        public OrdersService(IOrdersRepository repo, IMenuRepository menuRepo, IUserRepository userRepo, IPaymentsRepository payRepo, IUnitOfWork uow)
        {
            _repo = repo;
            _uow = uow;
            _userRepo = userRepo;
            _menuRepo = menuRepo;
            _payRepo = payRepo;
        }
        public async Task<PagedResponse<OrderListItemDto>> GetOrdersAsync(GetOrdersQuery query, CancellationToken ct)
        {
            if (query.BranchId <= 0)
                return new PagedResponse<OrderListItemDto>(
                    false, "branchId is required.", Array.Empty<OrderListItemDto>(),
                    Page: 1, PageSize: 0, TotalCount: 0, TotalPages: 0, HasNext: false, HasPrevious: false);

            // Clamp paging
            const int minSize = 5;
            const int maxSize = 100;
            var page = query.Page <= 0 ? 1 : query.Page;
            var pageSize = query.PageSize <= 0 ? 20 : Math.Clamp(query.PageSize, minSize, maxSize);

            // Get orders page
            var (orders, totalCount) = await _repo.GetOrdersPagedAsync(
                query with { Page = page, PageSize = pageSize }, ct);

            var totalPages = totalCount == 0 ? 0 : (int)Math.Ceiling(totalCount / (double)pageSize);

            // ✅ get clientId from branch (needed for menu modifiers)
            var branch = await _menuRepo.GetBranchAsync(query.BranchId, ct);
            if (branch == null)
                return new PagedResponse<OrderListItemDto>(
                    false, "Branch not found.", Array.Empty<OrderListItemDto>(),
                    Page: page, PageSize: pageSize, TotalCount: totalCount, TotalPages: totalPages,
                    HasNext: page < totalPages, HasPrevious: page > 1);

            var clientId = branch.ClientId;
            var itemIds = orders
                .SelectMany(o => o.OrderItems)
                .Select(i => i.ItemId)
                .Distinct()
                .ToList();

            // Fetch Items from Menu DB
            Dictionary<int, (string NameEn, string NameAr)> itemLookup;

            if (itemIds.Count == 0)
            {
                itemLookup = new Dictionary<int, (string, string)>();
            }
            else
            {
                var items = await _menuRepo.GetItemsByIdsAsync(clientId, itemIds, ct);

                itemLookup = items.ToDictionary(
                    i => i.ItemId,
                    i => (i.NameEn, i.NameAr)
                );
            }
            // Collect modifier ids (use o.Items consistently)
            var modifierIds = orders
                .SelectMany(o => o.OrderItems)
                .SelectMany(i => i.Modifiers)
                .Select(m => m.ModifierId)
                .Distinct()
                .ToList();

            var userIds = orders.Where(o => o.UserId.HasValue).Select(o => o.UserId!.Value).Distinct().ToList();

            var userLookup = await _userRepo.GetNamesByIdsAsync(userIds, ct);

            

            // Fetch modifiers master data (only if needed)
            Dictionary<int, (string NameEn, string NameAr, decimal BasePrice)> modifierLookup;

            if (modifierIds.Count == 0)
            {
                modifierLookup = new Dictionary<int, (string, string, decimal)>();
            }
            else
            {
                var modifiers = await _menuRepo.GetModifiersByIdsAsync(clientId, modifierIds, ct);

                modifierLookup = modifiers.ToDictionary(
                    m => m.Id,
                    m => (m.NameEn, m.NameAr, m.BasePrice)
                );
            }

            // Map response
            var data = orders.Select(o =>
            {
                // Resolve user names safely
                var userNames = (NameEn: (string?)null, NameAr: (string?)null);
                if (o.UserId.HasValue)
                    userLookup.TryGetValue(o.UserId.Value, out userNames);

                return new OrderListItemDto(
                    OrderId: o.OrderId,
                    OrderNumber: o.OrderNumber,
                    BranchId: o.BranchId,

                    UserId: o.UserId,
                    UserNameEn: userNames.NameEn,
                    UserNameAr: userNames.NameAr,

                    ShiftId: o.ShiftId,
                    Status: o.Status,
                    SubTotal: o.SubTotal,
                    TaxAmount: o.TaxAmount,
                    TotalAmount: o.TotalAmount,
                    CreatedAt: o.CreatedAt,
                    Note: o.Note,

                    Items: o.OrderItems.Select(i =>
                    {
                        var itemMaster = itemLookup.TryGetValue(i.ItemId, out var im)
                            ? im
                            : (NameEn: (string?)null, NameAr: (string?)null);

                        return new OrderItemDto(
                            OrderItemId: i.OrderItemId,
                            ItemId: i.ItemId,
                            NameEn: itemMaster.NameEn,
                            NameAr: itemMaster.NameAr,
                            Qty: i.Qty,
                            Price: i.Price,
                            Note: i.Note,
                            OrderItemModifier: i.Modifiers.Select(m =>
                            {
                                if (!modifierLookup.TryGetValue(m.ModifierId, out var mod))
                                    mod = (NameEn: (string?)null, NameAr: (string?)null, BasePrice: 0m);

                                return new OrderItemModifierDto(
                                    OrderItemModifierId: m.OrderItemModifierId,
                                    ModifierGroupId: m.ModifierGroupId,
                                    ModifierId: m.ModifierId,
                                    NameEn: mod.NameEn,
                                    NameAr: mod.NameAr,
                                    BasePrice: mod.BasePrice,
                                    UnitPrice: m.UnitPrice
                                );
                            }).ToList()
                        );
                    }).ToList()
                );
            }).ToList();

            return new PagedResponse<OrderListItemDto>(
                Success: true,
                Message: "Orders loaded successfully.",
                Data: data,
                Page: page,
                PageSize: pageSize,
                TotalCount: totalCount,
                TotalPages: totalPages,
                HasNext: page < totalPages,
                HasPrevious: page > 1
            );
        }

        public async Task<SuspendOrderResponse> SuspendAsync(SuspendOrderRequest request, CancellationToken ct)
        {
            if (request.OrderId <= 0)
                return new SuspendOrderResponse(false, "orderId is required.");

            if (request.BranchId <= 0)
                return new SuspendOrderResponse(false, "branchId is required.");

            if (string.IsNullOrWhiteSpace(request.DeviceId))
                return new SuspendOrderResponse(false, "deviceId is required.");

            var order = await _repo.GetByIdAsync(request.OrderId, ct);
            if (order is null)
                return new SuspendOrderResponse(false, "Order not found.");

            if (order.BranchId != request.BranchId)
                return new SuspendOrderResponse(false, "Order does not belong to this branch.");

            // Only allow Processing -> Suspended
            if (!string.Equals(order.Status, "Processing", StringComparison.OrdinalIgnoreCase))
                return new SuspendOrderResponse(false, $"Order status must be Processing. Current status: {order.Status}");

            order.Status = "Suspended";

            // Optional: if you have Notes column on Orders, persist it
            // order.Notes = request.Notes;

            // Begin unit-of-work (BeginAsync returns Task, not a transaction object)
            await _uow.BeginAsync(ct);
            _repo.Update(order);
            await _uow.SaveChangesAsync(ct);
            await _uow.CommitAsync(ct);

            return new SuspendOrderResponse(true, "Order suspended successfully.");
        }

        public async Task<SuspendOrderCreateResponse> CreateSuspendedAsync(SuspendOrderCreateRequest request, CancellationToken ct)
        {
            // Validation
            if (request.BranchId <= 0)
                return new(false, "BranchId is required.", null, null);

            if (request.PriceListId <= 0)
                return new(false, "PriceListId is required.", null, null);

            if (string.IsNullOrWhiteSpace(request.DeviceId))
                return new(false, "DeviceId is required.", null, null);

            if (request.Items is null || request.Items.Count == 0)
                return new(false, "Items are required.", null, null);

            if (request.Items.Any(i => i.ItemId <= 0 || i.Qty <= 0))
                return new(false, "Each item must have valid ItemId and Qty > 0.", null, null);

            // Branch -> clientId
            var branch = await _menuRepo.GetBranchAsync(request.BranchId, ct);
            if (branch is null)
                return new(false, "Branch not found.", null, null);

            var clientId = branch.ClientId;

            // Load item masters
            var itemIds = request.Items.Select(x => x.ItemId).Distinct().ToList();
            var itemsMaster = await _menuRepo.GetItemsByIdsAsync(clientId, itemIds, ct);

            if (itemsMaster.Count != itemIds.Count)
                return new(false, "One or more items not found.", null, null);

            // Item price overrides for selected pricelist (if you have it)
            // Expected: Dictionary<int itemId, decimal price>
            var itemPriceOverrides = await _menuRepo.GetItemPricesForPriceListAsync(itemIds, request.PriceListId, ct);

            // Tax rules (if you use)
            var taxRules = await _menuRepo.GetTaxRulesByClientAsync(clientId, ct);
            var taxRateByRuleId = taxRules.ToDictionary(t => t.TaxRuleId, t => t.Rate);

            decimal subTotal = 0m;
            decimal taxAmount = 0m;

            // Build OrderItems + OrderItemModifiers (snapshot)
            var orderItems = new List<OrderItems>();
            var orderItemModifiers = new List<OrderItemModifier>();

            foreach (var reqItem in request.Items)
            {
                var item = itemsMaster.First(x => x.ItemId == reqItem.ItemId);

                // Base unit item price
                var unitItemPrice = itemPriceOverrides.TryGetValue(item.ItemId, out var overridePrice)
                    ? overridePrice
                    : item.BasePrice;

                // Modifiers (unit sum + rows)
                var (modsUnitTotal, modRows) = await BuildModifiersSnapshotAsync(
                    clientId, request.PriceListId, reqItem, ct);

                var unitTotal = Round(unitItemPrice + modsUnitTotal);

                var lineSub = Round(unitTotal * reqItem.Qty);
                subTotal += lineSub;

                // tax (simple per item)
                if (item.TaxRuleId.HasValue &&
                    taxRateByRuleId.TryGetValue(item.TaxRuleId.Value, out var rate) &&
                    rate > 0)
                {
                    taxAmount += Round(lineSub * rate);
                }

                var orderItem = new OrderItems
                {
                    ItemId = item.ItemId,
                    Qty = reqItem.Qty,
                    Price = unitTotal,          // store unit total (item+mods)
                    Note = reqItem.Notes
                };

                orderItems.Add(orderItem);

                // attach temporary reference so we can set OrderItemId after insert
                foreach (var r in modRows)
                {
                    r._tempOrderItem = orderItem; // internal temp reference (see entity below)
                    orderItemModifiers.Add(r);
                }
            }

            var totalAmount = Round(subTotal + taxAmount);

            // Create Order (MATCHES YOUR TABLE COLUMNS)
            var order = new Orders
            {
                OrderNumber = GenerateOrderNumber(),  // no NextOrderNumberAsync (per your note)
                BranchId = request.BranchId,
                UserId = request.UserId,
                ShiftId = request.ShiftId,
                Status = "Suspended",

                SubTotal = Round(subTotal),
                TaxAmount = Round(taxAmount),
                TotalAmount = totalAmount,

                CreatedAt = DateTime.UtcNow,
                RowGuid = Guid.NewGuid(),

                DeviceId = request.DeviceId,
                SyncedAt = null,
                IsSynced = false,

                Note = request.Notes
            };

            // Begin unit-of-work. BeginAsync returns Task (no transaction object).
            await _uow.BeginAsync(ct);

            // Insert order header (get OrderId)
            _repo.AddOrder(order);

            // Set FK on items
            foreach (var oi in orderItems)
                oi.Order = order; // let EF set OrderId automatically (or set oi.OrderId after Save)

            _repo.AddOrderItems(orderItems);

            await _uow.SaveChangesAsync(ct); // generates OrderId + OrderItemIds

            // Now set OrderItemId for modifier rows
            foreach (var m in orderItemModifiers)
            {
                m.OrderItemId = m._tempOrderItem!.OrderItemId;
                m._tempOrderItem = null;
            }

            _repo.AddOrderItemModifiers(orderItemModifiers);

            await _uow.SaveChangesAsync(ct);
            // Commit using the unit-of-work instance
            await _uow.CommitAsync(ct);

            return new(true, "Order suspended and saved successfully.", order.OrderId, order.OrderNumber);
        }

        private async Task<(decimal ModsTotalUnit, List<OrderItemModifier> Rows)> BuildModifiersSnapshotAsync(
            int clientId,
            int priceListId,
            SuspendOrderItemRequest reqItem,
            CancellationToken ct)
        {
            // If there are no modifier groups on the request item, nothing to do.
            if (reqItem.ModifierGroups is null || reqItem.ModifierGroups.Count == 0)
                return (0m, new List<OrderItemModifier>());

            // Collect modifier ids from all groups
            var modifierIds = reqItem.ModifierGroups
                .SelectMany(g => g.Modifiers ?? Array.Empty<int>())
                .Distinct()
                .ToList();

            if (modifierIds.Count == 0)
                return (0m, new List<OrderItemModifier>());

            // Fetch modifiers master
            var modifiers = await _menuRepo.GetModifiersByIdsAsync(clientId, modifierIds, ct);
            var modLookup = modifiers.ToDictionary(x => x.Id);

            // Price overrides for modifiers for this price list
            var overridePrices = await _menuRepo.GetModifierPricesForPriceListAsync(modifierIds, priceListId, ct);

            decimal totalUnit = 0m;
            var rows = new List<OrderItemModifier>();

            foreach (var group in reqItem.ModifierGroups)
            {
                if (group.Modifiers is null) continue;

                foreach (var modId in group.Modifiers)
                {
                    if (!modLookup.TryGetValue(modId, out var mod))
                        continue; // unknown modifier -> ignore

                    var unitPrice = overridePrices.TryGetValue(modId, out var p) ? p : mod.BasePrice;

                    totalUnit += unitPrice;

                    rows.Add(new OrderItemModifier
                    {
                        ModifierGroupId = group.ModifierGroupId,
                        ModifierId = modId,
                        UnitPrice = Round(unitPrice)
                    });
                }
            }

            return (Round(totalUnit), rows);
        }

        private static decimal Round(decimal v) => Math.Round(v, 2, MidpointRounding.AwayFromZero);
        private static string GenerateOrderNumber()
        => $"S-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid():N}"[..22];

        public async Task<VisaOrdersResponse> GetVisaOrdersAsync(GetVisaOrdersRequest request, CancellationToken ct)
        {
            if (request.BranchId <= 0)
                return new VisaOrdersResponse(false, "BranchId is required.", Array.Empty<VisaOrderDto>());

            if (string.IsNullOrWhiteSpace(request.DeviceId))
                return new VisaOrdersResponse(false, "DeviceId is required.", Array.Empty<VisaOrderDto>());

            // clientId from branch
            var branch = await _menuRepo.GetBranchAsync(request.BranchId, ct);
            if (branch is null)
                return new VisaOrdersResponse(false, "Branch not found.", Array.Empty<VisaOrderDto>());

            var clientId = branch.ClientId;

            // 1) VISA methods
            var visaMethodIds = await _payRepo.GetVisaPaymentMethodIdsAsync(clientId, ct);
            if (visaMethodIds.Count == 0)
                return new VisaOrdersResponse(true, "No VISA payment methods found.", Array.Empty<VisaOrderDto>());

            // 2) OrderIds that have VISA payments
            var orderIds = await _payRepo.GetOrderIdsWithPaymentMethodsAsync(visaMethodIds, ct);
            if (orderIds.Count == 0)
                return new VisaOrdersResponse(true, "No VISA orders found.", Array.Empty<VisaOrderDto>());

            // 3) Load orders from Orders DB (filtered by branch + optional time/shift)
            var orders = await _repo.GetOrdersByIdsAsync(
                branchId: request.BranchId,
                orderIds: orderIds,
                fromUtc: request.FromUtc,
                toUtc: request.ToUtc,
                shiftId: request.ShiftId,
                ct: ct);

            if (orders.Count == 0)
                return new VisaOrdersResponse(true, "No VISA orders found for this branch/filters.", Array.Empty<VisaOrderDto>());

            var finalOrderIds = orders.Select(o => o.OrderId).ToList();

            // 4) Payments grouped by order (VISA only)
            var paymentsByOrder = await _payRepo.GetPaymentsForOrdersAsync(finalOrderIds, visaMethodIds, ct);

            // 5) Payment method names/types lookup
            var methodInfo = await _payRepo.GetPaymentMethodInfoAsync(clientId, visaMethodIds, ct);

            static decimal R(decimal v) => Math.Round(v, 2, MidpointRounding.AwayFromZero);

            var result = orders.Select(o =>
            {
                paymentsByOrder.TryGetValue(o.OrderId, out var pays);
                pays ??= new List<PaymentRow>();

                var visaPays = pays.Select(p =>
                {
                    methodInfo.TryGetValue(p.PaymentMethodId, out var info);
                    var methodName = info.MethodName ?? "Visa";
                    var methodType = info.MethodType ?? "Visa";

                    return new VisaPaymentDto(
                        PaymentId: p.PaymentId,
                        PaymentMethodId: p.PaymentMethodId,
                        MethodName: methodName,
                        MethodType: methodType,
                        Amount: R(p.Amount),
                        CreatedAt: p.CreatedAt,
                        DeviceId: p.DeviceId
                    );
                }).ToList();

                var paid = R(visaPays.Sum(x => x.Amount));

                return new VisaOrderDto(
                    OrderId: o.OrderId,
                    OrderNumber: o.OrderNumber,
                    BranchId: o.BranchId,
                    ShiftId: o.ShiftId,
                    UserId: o.UserId,
                    Status: o.Status,
                    SubTotal: R(o.SubTotal),
                    TaxAmount: R(o.TaxAmount),
                    TotalAmount: R(o.TotalAmount),
                    CreatedAt: o.CreatedAt,
                    VisaPaidAmount: paid,
                    VisaPayments: visaPays
                );
            }).OrderByDescending(x => x.CreatedAt).ToList();

            return new VisaOrdersResponse(true, "VISA orders loaded successfully.", result);
        }
    }
}
