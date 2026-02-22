using Fodo.Application.Features.Cash.CashIn;
using Fodo.Application.Features.Cash.CashOut;
using Fodo.Application.Implementation.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Fodo.API.Controllers
{
    [ApiController]
    [Route("api/Cash")]
    public class CashController : ControllerBase
    {
        private readonly ICashService _service;

        public CashController(ICashService service)
        {
            _service = service;
        }

        [HttpPost("Cash-In")]
        [ProducesResponseType(typeof(CreateCashInResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CreateCashInResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CreateCashInResponse>> Create([FromBody] CreateCashInRequest request, CancellationToken ct)
        {
            if (request is null)
                return BadRequest(new CreateCashInResponse(false, "Request body is required.", null, null, null));

            var result = await _service.CreateAsync(request, ct);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("Cash-Out")]
        [ProducesResponseType(typeof(CreateCashOutResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CreateCashOutResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CreateCashOutResponse>> Create([FromBody] CreateCashOutRequest request, CancellationToken ct)
        {
            if (request is null)
                return BadRequest(new CreateCashOutResponse(false, "Request body is required.", null, null, null));

            var result = await _service.CreateAsync(request, ct);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
