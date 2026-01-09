using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Application.Handlers
{
    public class LoginHandler
    {
        public async Task<LoginResponseDto> Handle(LoginCommand command)
        {
            var user = await _userRepo.GetByUsername(command.Username);

            if (user == null || !user.IsActive)
                throw new UnauthorizedException();

            if (!PasswordHasher.Verify(command.Password, user.PasswordHash))
                throw new UnauthorizedException();

            if (!user.UserBranches.Any(b => b.BranchId == command.BranchId))
                throw new ForbiddenException();

            var permissions = user.Role.Permissions
                .Select(p => p.Permission.Code)
                .ToList();

            return TokenFactory.Create(user, permissions);
        }
    }

}
