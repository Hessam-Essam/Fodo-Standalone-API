using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Application.Features.Shifts.CloseShift
{
    public sealed record CloseShiftResponse(
    bool Success,
    string Message
);

}
