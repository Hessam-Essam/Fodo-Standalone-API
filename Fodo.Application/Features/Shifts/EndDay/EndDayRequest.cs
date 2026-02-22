using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Application.Features.Shifts.EndDay
{
    public sealed record EndDayRequest(
    int BranchId,
    int ShiftId,
    string DeviceId,
    decimal ClosingCash
);
}
