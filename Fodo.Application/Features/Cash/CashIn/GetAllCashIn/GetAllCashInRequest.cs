using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Application.Features.Cash.CashIn.GetAllCashIn
{
    public sealed record GetAllCashInRequest(
    int BranchId,
    int ShiftId,
    string? DeviceId = null
);
}
