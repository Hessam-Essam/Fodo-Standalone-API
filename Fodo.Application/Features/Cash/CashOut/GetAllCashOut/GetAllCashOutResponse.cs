using Fodo.Contracts.DTOS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Application.Features.Cash.CashOut.GetAllCashOut
{
    public sealed record GetAllCashOutResponse(
    bool Success,
    string Message,
    IReadOnlyList<CashOutVoucherDto> Data
);
}
