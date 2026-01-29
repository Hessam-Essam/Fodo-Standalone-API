using Fodo.Contracts.DTOS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Application.Implementation.Interfaces
{
    public interface IBranchesService
    {
        Task<ClientsDto> GetByClientCodeAsync(string clientCode, CancellationToken ct);
    }
}
