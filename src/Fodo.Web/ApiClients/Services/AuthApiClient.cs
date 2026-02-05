using Fodo.API.Responses;
using Fodo.Application.Features.Login;
using Fodo.Contracts.Requests;
using Fodo.Web.ApiClients.Interfaces;

namespace Fodo.Web.ApiClients.Services
{
    public class AuthApiClient : IAuthApiClient
    {
        private readonly HttpClient _http;

        public AuthApiClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<ApiResponse<LoginResponse>> LoginPortalAsync(string username, string password)
        {
            var res = await _http.PostAsJsonAsync("api/auth/LoginPortal", new LoginByPasswordRequest
            {
                Username = username,
                Password = password
            });

            // API returns { code, body }
            var payload = await res.Content.ReadFromJsonAsync<ApiResponse<LoginResponse>>();

            return payload ?? new ApiResponse<LoginResponse>(500, new LoginResponse
            {
                Success = false,
                Message = "Invalid response from server."
            });
        }
    }

}
