using Fodo.API.Responses;
using Fodo.Application.Features.Login;

namespace Fodo.Web.ApiClients.Interfaces
{
    public interface IAuthApiClient
    {
        Task<ApiResponse<LoginResponse>> LoginPortalAsync(string username, string password);
    }
}
