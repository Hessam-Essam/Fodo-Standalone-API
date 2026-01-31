using Fodo.Contracts.DTOS;
using Fodo.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Application.Implementation.Interfaces
{
    public interface IBranchesService
    {
        Task<ClientsDto> GetByClientCodeAsync(string clientCode, CancellationToken ct);
        Task<BranchCatalogResponse> GetBranchCatalogAsync(int branchId);
    }
}
