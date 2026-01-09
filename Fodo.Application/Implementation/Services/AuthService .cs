using Fodo.Application.Implementation.Interfaces;
using Fodo.Application.Implementation.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Fodo.Application.Implementation.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenGenerator _jwt;

        public AuthService(
            IUserRepository userRepository,
            IJwtTokenGenerator jwt)
        {
            _userRepository = userRepository;
            _jwt = jwt;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            // 1️⃣ Validate password format
            if (!Regex.IsMatch(request.Password, @"^\d{6}$"))
                throw new UnauthorizedAccessException("Invalid password format");

            // 2️⃣ Get user
            var user = await _userRepository.GetLoginAsync(request.Username);

            if (user == null || !user.IsActive)
                throw new UnauthorizedAccessException("Invalid credentials");

            // 3️⃣ Verify password
            if (!PasswordHasher.Verify(request.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid credentials");

            // 4️⃣ Validate branch access
            if (!user.UserBranches.Any(b => b.BranchId == request.BranchId))
                throw new UnauthorizedAccessException("Branch access denied");

            // 5️⃣ Validate role
            if (!user.Role.IsActive)
                throw new UnauthorizedAccessException("Role inactive");

            // 6️⃣ Load permissions
            var permissions = user.Role.Permissions
                .Select(p => p.Permission.Code)
                .ToList();

            // 7️⃣ Generate token
            var token = _jwt.Generate(user, permissions, request.BranchId);

            return new LoginResponse
            {
                Token = token,
                User = new LoggedUserDto
                {
                    Id = user.Id,
                    FullName = user.FullNameEn,
                    RoleName = user.Role.NameEn
                },
                Permissions = permissions
            };
        }
    }

}
