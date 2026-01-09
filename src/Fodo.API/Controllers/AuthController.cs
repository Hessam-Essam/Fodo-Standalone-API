using Microsoft.AspNetCore.Mvc;

namespace Fodo.API.Controllers
{
    [ApiController]
    [Route("api/pos/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IPosAuthService _authService;

        public PosAuthController(IPosAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] PosLoginRequest request)
        {
            var result = await _authService.LoginAsync(request);
            return Ok(result);
        }
    }

}
