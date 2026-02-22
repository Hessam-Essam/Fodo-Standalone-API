using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Application.Features.Shifts.StartShift
{
    public sealed record StartShiftRequest(
    int BranchId,
    Guid UserId,
    string DeviceId,
    decimal OpeningCash,
    DateTime? StartTimeUtc // optional: if null -> server uses UtcNow
);
}
