using Fodo.Application.Features.Cash.CashIn.CreateCashIn;
using Fodo.Application.Features.Cash.CashIn.GetAllCashIn;
using Fodo.Application.Features.Cash.CashOut.CreateCashOut;
using Fodo.Application.Features.Cash.CashOut.GetAllCashOut;
using Fodo.Application.Implementation.Interfaces;
using Fodo.Contracts.DTOS;
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

        [HttpPost("Cash-In-All")]
        [ProducesResponseType(typeof(GetAllCashInResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GetAllCashInResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetAllCashInResponse>> GetAll([FromBody] GetAllCashInRequest request, CancellationToken ct)
        {
            if (request is null)
                return BadRequest(new GetAllCashInResponse(false, "Request body is required.", Array.Empty<CashInVoucherDto>()));

            var result = await _service.GetAllAsync(request, ct);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("Cash-Out-All")]
        [ProducesResponseType(typeof(GetAllCashOutResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GetAllCashOutResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetAllCashOutResponse>> GetAll([FromBody] GetAllCashOutRequest request, CancellationToken ct)
        {
            if (request is null)
                return BadRequest(new GetAllCashOutResponse(false, "Request body is required.", Array.Empty<CashOutVoucherDto>()));

            var result = await _service.GetAllAsync(request, ct);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
