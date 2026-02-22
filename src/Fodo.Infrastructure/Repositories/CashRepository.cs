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
    }
}
