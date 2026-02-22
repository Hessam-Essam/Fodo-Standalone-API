using Fodo.Application.Features.Shifts;
using Fodo.Application.Features.Shifts.CloseShift;
using Fodo.Application.Features.Shifts.EndDay;
using Fodo.Application.Features.Shifts.StartShift;
using Fodo.Application.Implementation.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Fodo.API.Controllers
{
    [ApiController]
    [Route("api/Shifts")]
    public class ShiftsController : ControllerBase
    {
        private readonly IShiftService _service;

        public ShiftsController(IShiftService service)
        {
            _service = service;
        }

        [HttpPost("Start")]
        [ProducesResponseType(typeof(StartShiftResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(StartShiftResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<StartShiftResponse>> Start([FromBody] StartShiftRequest request, CancellationToken ct)
        {
            var result = await _service.StartShiftAsync(request, ct);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("Close")]
        public async Task<ActionResult<CloseShiftResponse>> Close([FromBody] CloseShiftRequest request,CancellationToken ct)
        {
            var result = await _service.CloseShiftAsync(request, ct);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("Visa-Total")]
        [ProducesResponseType(typeof(ShiftVisaTotalResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ShiftVisaTotalResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ShiftVisaTotalResponse>> GetShiftVisaTotal(
       [FromBody] ShiftVisaTotalRequest request,
       CancellationToken ct)
        {
            if (request is null)
                return BadRequest(new ShiftVisaTotalResponse(false, "Request body is required.", 0, 0, default, default, 0m));

            var result = await _service.GetVisaTotalAsync(request, ct);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("end-day")]
        [ProducesResponseType(typeof(EndDayResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(EndDayResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EndDayResponse>> EndDay([FromBody] EndDayRequest request, CancellationToken ct)
        {
            if (request is null)
                return BadRequest(new EndDayResponse(false, "Request body is required.", 0, 0, "", default, 0, 0,
                    new DayAccountsDto(0, 0, 0, 0, 0), new DayTotalsDto(0, 0, null)));

            var result = await _service.EndDayAsync(request, ct);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }

}
