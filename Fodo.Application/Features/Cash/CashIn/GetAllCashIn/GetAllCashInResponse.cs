using Fodo.Contracts.DTOS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Application.Features.Cash.CashIn.GetAllCashIn
{
    public sealed record GetAllCashInResponse(
    bool Success,
    string Message,
    IReadOnlyList<CashInVoucherDto> Data
);
}
