using Fodo.Application.Features.Shifts;
using Fodo.Application.Features.Shifts.CloseShift;
using Fodo.Application.Features.Shifts.EndDay;
using Fodo.Application.Features.Shifts.StartShift;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Application.Implementation.Interfaces
{
    public interface IShiftService
    {
        Task<StartShiftResponse> StartShiftAsync(StartShiftRequest request, CancellationToken ct);
        Task<CloseShiftResponse> CloseShiftAsync(CloseShiftRequest request, CancellationToken ct);
        Task<ShiftVisaTotalResponse> GetVisaTotalAsync(ShiftVisaTotalRequest request, CancellationToken ct);

        Task<EndDayResponse> EndDayAsync(EndDayRequest request, CancellationToken ct);
    }
}
