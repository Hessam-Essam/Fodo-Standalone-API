using Fodo.Application.Implementation.IRepositories;
using Fodo.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Infrastructure.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly IdentityDbContext _db;
        public ClientRepository(IdentityDbContext db) => _db = db;

        public async Task<int?> GetClientIdByCodeAsync(string clientCode, CancellationToken ct)
        {
            return await _db.Clients
                .Where(c => c.ClientCode == clientCode)
                .Select(c => (int?)c.ClientId)
                .SingleOrDefaultAsync(ct);
        }
    }
}
