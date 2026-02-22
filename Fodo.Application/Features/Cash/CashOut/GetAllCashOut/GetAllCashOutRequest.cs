using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Application.Features.Cash.CashOut.GetAllCashOut
{
    public sealed record GetAllCashOutRequest(
    int BranchId,
    int ShiftId,
    string? DeviceId = null
);
}
