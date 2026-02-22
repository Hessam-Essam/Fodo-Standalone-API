using Fodo.Application.Features.Orders;
using Fodo.Application.Features.Pagination;
using Fodo.Application.Features.Payment;
using Fodo.Application.Implementation.Interfaces;
using Fodo.Application.Implementation.Services;
using Fodo.Contracts.DTOS;
using Microsoft.AspNetCore.Mvc;

namespace Fodo.API.Controllers
{
    [ApiController]
    [Route("api/Orders")]
    public class OrdersController : Controller
    {
        private readonly IPlaceOrderService _placeOrderService;
        private readonly IOrdersService _ordersService;

        public OrdersController(IPlaceOrderService placeOrderService, IOrdersService ordersService)
        {
            _placeOrderService = placeOrderService;
            _ordersService = ordersService;
        }

        /// <summary>
        /// Submit/Place order from mobile app (no payment).
        /// </summary>
        [HttpPost("PlaceOrder")]
        [ProducesResponseType(typeof(PlaceOrderResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(PlaceOrderResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PlaceOrderResponse>> PlaceOrder([FromBody] PlaceOrderRequest request,CancellationToken ct)
        {
            if (request is null)
                return BadRequest(new PlaceOrderResponse(false, "Request body is required.", null, null));

            var result = await _placeOrderService.PlaceOrderAsync(request, ct);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("SuspendOrder")]
        [ProducesResponseType(typeof(SuspendOrderResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SuspendOrderResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SuspendOrderResponse>> Suspend(
       [FromBody] SuspendOrderRequest request,
       CancellationToken ct)
        {
            if (request is null)
                return BadRequest(new SuspendOrderResponse(false, "Request body is required."));

            var result = await _ordersService.SuspendAsync(request, ct);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("CreateSuspendOrder")]
        [ProducesResponseType(typeof(SuspendOrderCreateResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SuspendOrderCreateResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SuspendOrderCreateResponse>> Suspend([FromBody] SuspendOrderCreateRequest request, CancellationToken ct)
        {
            if (request is null)
                return BadRequest(new SuspendOrderCreateResponse(false, "Request body is required.", null, null));

            var result = await _ordersService.CreateSuspendedAsync(request, ct);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        // GET /api/v1/orders?branchId=2&shiftId=1&status=Processing&page=1&pageSize=20

        [HttpPost("GetAllOrders")]
        [ProducesResponseType(typeof(PagedResponse<OrderListItemDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(PagedResponse<OrderListItemDto>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PagedResponse<OrderListItemDto>>> List(
        [FromBody] GetOrdersQuery request,
        CancellationToken ct)
        {
            if (request is null)
                return BadRequest(new PagedResponse<OrderListItemDto>(
                    false, "Request body is required.", Array.Empty<OrderListItemDto>(),
                    Page: 1, PageSize: 0, TotalCount: 0, TotalPages: 0, HasNext: false, HasPrevious: false));

            var query = new GetOrdersQuery(
                BranchId: request.BranchId,
                ShiftId: request.ShiftId,
                Status: request.Status,
                FromUtc: request.FromUtc,
                ToUtc: request.ToUtc,
                Page: request.Page,
                PageSize: request.PageSize
            );

            var result = await _ordersService.GetOrdersAsync(query, ct);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("GetVisaOrders")]
        public async Task<ActionResult<VisaOrdersResponse>> ListVisaOrders(
        [FromBody] GetVisaOrdersRequest request,
        CancellationToken ct)
        {
            if (request is null)
                return BadRequest(new VisaOrdersResponse(false, "Request body is required.", Array.Empty<VisaOrderDto>()));

            var result = await _ordersService.GetVisaOrdersAsync(request, ct);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

    }
}
