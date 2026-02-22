using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Application.Features.Cash.CashIn
{
    public sealed record CreateCashInResponse(
    bool Success,
    string Message,
    long? CashInVoucherId,
    Guid? RowGuid,
    DateTime? CreatedAtUtc
);
}
