using Fodo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Application.Implementation.IRepositories
{
    public interface IShiftRepository
    {
        Task<Shifts?> GetOpenShiftAsync(int branchId, Guid userId, CancellationToken ct);
        Task<int> CreateAsync(Shifts shift, CancellationToken ct);
        Task<Shifts?> GetByIdAsync(int shiftId, CancellationToken ct);
        Task UpdateAsync(Shifts shift, CancellationToken ct);
    }
}
