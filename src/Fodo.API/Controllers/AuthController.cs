using Fodo.Application.Implementation.Interfaces;
using Fodo.Contracts.Requests;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace Fodo.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService) => _authService = authService;

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginByPasswordRequest request)
        {
            var result = await _authService.LoginAsync(request);

            if (!result.Success)
            {
                if (result.Message.Contains("required") || result.Message.Contains("6 digits"))
                    return BadRequest(result);

                return Unauthorized(result);
            }

            return Ok(result);
        }
    }

}
