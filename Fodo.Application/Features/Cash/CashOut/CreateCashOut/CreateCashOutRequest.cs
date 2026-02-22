using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Application.Features.Cash.CashOut.CreateCashOut
{
    public sealed record CreateCashOutRequest(
    int BranchId,
    int ShiftId,
    Guid? UserId,
    string DeviceId,
    decimal Amount,
    string? Reason
);
}
