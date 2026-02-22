using Fodo.Application.Implementation.IRepositories;
using Fodo.Domain.Entities;
using Fodo.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Infrastructure.Repositories
{
    public class CashRepository : ICashRepository
    {
        private readonly IdentityDbContext _db;

        public CashRepository(IdentityDbContext db)
        {
            _db = db;
        }

        public Task<Shifts?> GetShiftAsync(int shiftId, CancellationToken ct)
            => _db.Shifts.AsNoTracking().FirstOrDefaultAsync(s => s.ShiftId == shiftId, ct);

        public async Task AddAsync(CashInVoucher entity, CancellationToken ct)
        {
            await _db.CashInVouchers.AddAsync(entity, ct);
        }

        public async Task AddAsync(CashOutVoucher entity, CancellationToken ct)
        {
            await _db.CashOutVouchers.AddAsync(entity, ct);
        }

        public async Task<List<CashInVoucher>> GetAllByShiftAsync(
        int branchId,
        int shiftId,
        string? deviceId,
        CancellationToken ct)
        {
            var q = _db.CashInVouchers
                .AsNoTracking()
                .Where(x => x.BranchId == branchId && x.ShiftId == shiftId);

            if (!string.IsNullOrWhiteSpace(deviceId))
                q = q.Where(x => x.DeviceId == deviceId);

            return await q
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(ct);
        }

        public async Task<List<CashOutVoucher>> GetAllCashOutByShiftAsync(
        int branchId,
        int shiftId,
        string? deviceId,
        CancellationToken ct)
        {
            var q = _db.CashOutVouchers
                .AsNoTracking()
                .Where(x => x.BranchId == branchId && x.ShiftId == shiftId);

            if (!string.IsNullOrWhiteSpace(deviceId))
                q = q.Where(x => x.DeviceId == deviceId);

            return await q
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(ct);
        }

        public async Task<decimal> SumCashInByShiftAsync(int branchId, int shiftId, CancellationToken ct)
        {
            return await _db.CashInVouchers
                .AsNoTracking()
                .Where(x => x.BranchId == branchId && x.ShiftId == shiftId)
                .SumAsync(x => (decimal?)x.Amount, ct) ?? 0m;
        }

        public async Task<decimal> SumCashOutByShiftAsync(int branchId, int shiftId, CancellationToken ct)
        {
            return await _db.CashOutVouchers
                .AsNoTracking()
                .Where(x => x.BranchId == branchId && x.ShiftId == shiftId)
                .SumAsync(x => (decimal?)x.Amount, ct) ?? 0m;
        }
    }
}
