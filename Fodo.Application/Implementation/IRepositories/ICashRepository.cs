using Fodo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Application.Implementation.IRepositories
{
    public interface ICashRepository
    {
        Task<Shifts?> GetShiftAsync(int shiftId, CancellationToken ct);
        Task AddAsync(CashInVoucher entity, CancellationToken ct);
        Task AddAsync(CashOutVoucher entity, CancellationToken ct);
        Task<List<CashInVoucher>> GetAllByShiftAsync(
        int branchId,
        int shiftId,
        string? deviceId,
        CancellationToken ct);

        Task<List<CashOutVoucher>> GetAllCashOutByShiftAsync(
        int branchId,
        int shiftId,
        string? deviceId,
        CancellationToken ct);

        Task<decimal> SumCashInByShiftAsync(int branchId, int shiftId, CancellationToken ct);
        Task<decimal> SumCashOutByShiftAsync(int branchId, int shiftId, CancellationToken ct);
    }
}
