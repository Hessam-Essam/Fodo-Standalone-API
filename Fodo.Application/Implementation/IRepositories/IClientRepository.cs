using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Application.Implementation.IRepositories
{
    public interface IClientRepository
    {
        Task<int?> GetClientIdByCodeAsync(string clientCode, CancellationToken ct);
    }
}
