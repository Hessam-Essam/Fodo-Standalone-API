using Fodo.Application.Features.Orders;
using Fodo.Application.Features.Pagination;
using Fodo.Application.Features.Payment;

namespace Fodo.Application.Implementation.Interfaces
{
    public interface IOrdersService
    {
        Task<PagedResponse<OrderListItemDto>> GetOrdersAsync(GetOrdersQuery query, CancellationToken ct);
        Task<SuspendOrderResponse> SuspendAsync(SuspendOrderRequest request, CancellationToken ct);
        Task<SuspendOrderCreateResponse> CreateSuspendedAsync(SuspendOrderCreateRequest request, CancellationToken ct);
        Task<VisaOrdersResponse> GetVisaOrdersAsync(GetVisaOrdersRequest request, CancellationToken ct);
    }
}
