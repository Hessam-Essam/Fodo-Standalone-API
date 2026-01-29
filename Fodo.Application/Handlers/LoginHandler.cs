using Fodo.Application.Common.Execptions;
using Fodo.Application.Features.Login;
using Fodo.Application.Implementation.IRepositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Fodo.Application.Handlers
{
    public class LoginHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly IUserRepository _userRepository;
        public LoginHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<LoginResponse> Handle(LoginCommand command, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetLoginAsync(command.Username);

            if (user == null || !user.IsActive)
                throw new UnauthorizedException();

            if (!PasswordHasher.Verify(command.Password, user.PasswordHash))
                throw new UnauthorizedException();

            //if (!user.UserBranches.Any(b => b.BranchId == command.BranchId))
            //    throw new ForbiddenException();

            var permissions = user.Role.Permissions
                .Select(p => p.Permission.Code)
                .ToList();

            return LoginResponse.Ok(
            user: new LoginUserDto
            {
                Id = user.UserId,
                Role = user.Role.NameEn
            });
        }
    }

    public static class PasswordHasher
    {
        public static string Hash(string pin)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(pin));
            return Convert.ToBase64String(bytes);
        }

        public static bool Verify(string pin, string hash)
        {
            return Hash(pin) == hash;
        }
    }

}
