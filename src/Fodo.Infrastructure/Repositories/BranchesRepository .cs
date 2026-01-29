using Fodo.Application.Implementation.IRepositories;
using Fodo.Contracts.DTOS;
using Fodo.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Fodo.Infrastructure.Repositories
{
    public class BranchesRepository : IBranchesRepository
    {
        private readonly IdentityDbContext _db;

        public BranchesRepository(IdentityDbContext db) => _db = db;

        public async Task<IReadOnlyList<BranchesDto>> GetByClientIdAsync(int clientId, CancellationToken ct)
        {
            return await _db.Branches
                .Where(b => b.ClientId == clientId)
                .OrderBy(b => b.BranchId)
                .Select(b => new BranchesDto(
                    b.BranchId,
                    b.Code,
                    b.NameEn,
                    b.NameAr,
                    b.Address,
                    b.Phone,
                    b.IsActive
                ))
                .ToListAsync(ct);
        }
    }

}
