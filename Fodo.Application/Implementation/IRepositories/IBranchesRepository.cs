using Fodo.Contracts.DTOS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Application.Implementation.IRepositories
{
    public interface IBranchesRepository
    {
        Task<IReadOnlyList<BranchesDto>> GetByClientIdAsync(int clientId, CancellationToken ct);
    }
}
