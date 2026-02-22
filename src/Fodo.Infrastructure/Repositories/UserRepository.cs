using Fodo.Application.Implementation.IRepositories;
using Fodo.Domain.Entities;
using Fodo.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Fodo.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IdentityDbContext _db;
        public UserRepository(IdentityDbContext db) => _db = db;

        public Task<List<User>> GetActiveUsersAsync()
        {
            return _db.Users
                .Where(u => u.IsActive)
                .ToListAsync();
        }

        public Task<User> GetByNormalizedUsernameAsync(string normalizedUsername)
        => _db.Users.FirstOrDefaultAsync(x => x.NormalizedUserName == normalizedUsername);

        public async Task<List<User>> GetUsersByIdsAsync(List<Guid> userIds, CancellationToken ct)
        {
            return await _db.Users
                .AsNoTracking()
                .Where(u => userIds.Contains(u.UserId))
                .ToListAsync(ct);
        }

        public async Task<Dictionary<Guid, (string? NameEn, string? NameAr)>> GetNamesByIdsAsync(
        IReadOnlyCollection<Guid> userIds,
        CancellationToken ct)
        {
            if (userIds == null || userIds.Count == 0)
                return new Dictionary<Guid, (string?, string?)>();

            return await _db.Users
                .AsNoTracking()
                .Where(u => userIds.Contains(u.UserId))
                .Select(u => new
                {
                    u.UserId,
                    NameEn = u.FullNameEn,
                    NameAr = u.FullNameAr 
                })
                .ToDictionaryAsync(x => x.UserId, x => ((string?)x.NameEn, (string?)x.NameAr), ct);
        }
    }

}
