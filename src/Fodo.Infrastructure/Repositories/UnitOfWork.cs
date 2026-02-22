using Fodo.Application.Implementation.IRepositories;
using Fodo.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Infrastructure.Repositories
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly IdentityDbContext _db;
        private IDbContextTransaction? _tx;

        public UnitOfWork(IdentityDbContext db) => _db = db;

        public async Task BeginAsync(CancellationToken ct)
            => _tx = await _db.Database.BeginTransactionAsync(ct);

        public async Task CommitAsync(CancellationToken ct)
        {
            await _db.SaveChangesAsync(ct);
            if (_tx != null) await _tx.CommitAsync(ct);
        }

        public async Task RollbackAsync(CancellationToken ct)
        {
            if (_tx != null) await _tx.RollbackAsync(ct);
        }

        public Task SaveChangesAsync(CancellationToken ct)
        => _db.SaveChangesAsync(ct);
    }
}
