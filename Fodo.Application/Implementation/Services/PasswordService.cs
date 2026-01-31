using Fodo.Application.Implementation.Interfaces;
using Fodo.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Application.Implementation.Services
{
    public class PasswordService : IPasswordService
    {
        private readonly PasswordHasher<User> _hasher = new PasswordHasher<User>();

        public bool Verify(User user, string plainPassword)
        {
            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, plainPassword);
            return result == PasswordVerificationResult.Success ||
                   result == PasswordVerificationResult.SuccessRehashNeeded;
        }
    }
}
