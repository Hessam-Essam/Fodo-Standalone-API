using Fodo.API.Responses;
using Fodo.Application.Implementation.Interfaces;
using Fodo.Contracts.DTOS;
using Fodo.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Fodo.API.Controllers
{
    [ApiController]
    [Route("api/branches")]
    public class BranchesController : ControllerBase
    {
        private readonly IBranchesService _service;
        public BranchesController(IBranchesService service) => _service = service;

        [HttpGet("GetByClientCode")]
        public async Task<ActionResult<ClientsDto>> GetByClientCode([FromQuery] string clientCode, CancellationToken ct)
        {
            var result = await _service.GetByClientCodeAsync(clientCode, ct);
            return result is null ? NotFound() : Ok(result);
        }

        [HttpGet("GetByBranchId")]
        public async Task<IActionResult> GetByBranchId(int branchId)
        {
            var result = await _service.GetBranchCatalogAsync(branchId);

            if (!result.Success)
                return BadRequest(new ApiResponse<BranchCatalogResponse>(400, result));

            return Ok(new ApiResponse<BranchCatalogResponse>(200, result));
        }
    }
}
