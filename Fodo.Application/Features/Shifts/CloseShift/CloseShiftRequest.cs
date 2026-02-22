using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Application.Features.Shifts.CloseShift
{
    public sealed record CloseShiftRequest(
    int ShiftId,
    int BranchId,
    string DeviceId,
    Guid UserId,
    decimal ClosingCash
);
}
