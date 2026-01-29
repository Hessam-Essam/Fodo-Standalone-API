using Fodo.Application.Implementation.Interfaces;
using Fodo.Contracts.DTOS;
using Microsoft.AspNetCore.Mvc;

namespace Fodo.API.Controllers
{
    [ApiController]
    [Route("api/devices")]
    public class DevicesController : ControllerBase
    {
        private readonly IDeviceVerificationService _service;

        public DevicesController(IDeviceVerificationService service)
        {
            _service = service;
        }

        [HttpPost("VerifyDevicePerBranch")]
        public async Task<IActionResult> VerifyDevicePerBranch([FromBody] DevicesDto request)
        {
            var result = await _service.VerifyAsync(request);

            if (!result.Success)
            {
                // if validation error => 400, if not found => 404
                if (result.Message.Contains("required"))
                    return BadRequest(result);

                return NotFound(result);
            }

            return Ok(result);
        }
    }
}
