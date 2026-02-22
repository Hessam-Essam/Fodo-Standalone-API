using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Application.Features.Cash.CashIn
{
    public sealed record CreateCashInRequest(
    int BranchId,
    int ShiftId,
    Guid? UserId,
    string DeviceId,
    decimal Amount,
    string? Reason
);
}
