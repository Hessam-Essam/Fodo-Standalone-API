using Fodo.Application.Features.Cash.CashIn.CreateCashIn;
using Fodo.Application.Features.Cash.CashIn.GetAllCashIn;
using Fodo.Application.Features.Cash.CashOut.CreateCashOut;
using Fodo.Application.Features.Cash.CashOut.GetAllCashOut;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Application.Implementation.Interfaces
{
    public interface ICashService
    {
        Task<CreateCashInResponse> CreateAsync(CreateCashInRequest request, CancellationToken ct);
        Task<CreateCashOutResponse> CreateAsync(CreateCashOutRequest request, CancellationToken ct);
        Task<GetAllCashInResponse> GetAllAsync(GetAllCashInRequest request, CancellationToken ct);
        Task<GetAllCashOutResponse> GetAllAsync(GetAllCashOutRequest request, CancellationToken ct);
    }
}
