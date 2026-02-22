using Fodo.Application.Implementation.IRepositories;
using Fodo.Domain.Entities;
using Fodo.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Infrastructure.Repositories
{
    public class ShiftRepository : IShiftRepository
    {
        private readonly IdentityDbContext _db;

        public ShiftRepository(IdentityDbContext db)
        {
            _db = db;
        }

        public Task<Shifts?> GetOpenShiftAsync(int branchId, Guid userId, CancellationToken ct)
        {
            return _db.Shifts.AsNoTracking()
                .FirstOrDefaultAsync(s => s.BranchId == branchId
                                       && s.UserId == userId
                                       && s.EndTime == null, ct);
        }

        public async Task<int> CreateAsync(Shifts shift, CancellationToken ct)
        {
            _db.Shifts.Add(shift);
            await _db.SaveChangesAsync(ct);
            return shift.ShiftId;
        }

        public Task<Shifts?> GetByIdAsync(int shiftId, CancellationToken ct)
        {
            return _db.Shifts
                .FirstOrDefaultAsync(s => s.ShiftId == shiftId, ct);
        }

        public Task UpdateAsync(Shifts shift, CancellationToken ct)
        {
            _db.Shifts.Update(shift);
            return Task.CompletedTask;
        }
    }
}
