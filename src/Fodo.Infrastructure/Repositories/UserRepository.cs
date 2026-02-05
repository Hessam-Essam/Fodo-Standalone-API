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
    }

}
