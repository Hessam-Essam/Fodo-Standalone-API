using Fodo.Application.Features.Cash.CashIn;
using Fodo.Application.Features.Cash.CashOut;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Application.Implementation.Interfaces
{
    public interface ICashService
    {
        Task<CreateCashInResponse> CreateAsync(CreateCashInRequest request, CancellationToken ct);
        Task<CreateCashOutResponse> CreateAsync(CreateCashOutRequest request, CancellationToken ct);
    }
}
