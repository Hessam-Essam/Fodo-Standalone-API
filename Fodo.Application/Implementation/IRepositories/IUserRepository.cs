using Fodo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Application.Implementation.IRepositories
{
    public interface IUserRepository
    {
        Task<User?> GetLoginAsync(string username);
    }
}
