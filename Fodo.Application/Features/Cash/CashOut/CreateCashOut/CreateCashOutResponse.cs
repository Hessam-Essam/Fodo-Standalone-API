using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Application.Features.Cash.CashOut.CreateCashOut
{
    public sealed record CreateCashOutResponse(
    bool Success,
    string Message,
    long? CashOutVoucherId,
    Guid? RowGuid,
    DateTime? CreatedAtUtc
);
}
