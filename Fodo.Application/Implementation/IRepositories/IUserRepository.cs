using Fodo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Application.Implementation.IRepositories
{
    public interface IUserRepository
    {
        Task<List<User>> GetActiveUsersAsync();
        Task<User> GetByNormalizedUsernameAsync(string normalizedUsername);
        Task<List<User>> GetUsersByIdsAsync(List<Guid> userIds, CancellationToken ct);
        Task<Dictionary<Guid, (string? NameEn, string? NameAr)>> GetNamesByIdsAsync(IReadOnlyCollection<Guid> userIds, CancellationToken ct);
    }
}
