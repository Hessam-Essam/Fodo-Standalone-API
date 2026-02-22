using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Application.Features.Shifts.StartShift
{
    public sealed record StartShiftResponse(
    bool Success,
    string Message,
    int? ShiftId
);
}
