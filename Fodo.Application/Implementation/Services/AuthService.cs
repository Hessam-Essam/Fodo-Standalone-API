using Fodo.Application.Features.Login;
using Fodo.Application.Handlers;
using Fodo.Application.Implementation.Interfaces;
using Fodo.Application.Implementation.IRepositories;
using Fodo.Contracts.DTOS;
using Fodo.Contracts.Requests;
using Fodo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Fodo.Application.Implementation.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;
        private readonly IPasswordService _passwordService;
        private readonly ITokenService _tokenService;

        public AuthService(IUserRepository userRepo, IPasswordService passwordService, ITokenService tokenService)
        {
            _userRepo = userRepo;
            _passwordService = passwordService;
            _tokenService = tokenService;
        }

        public async Task<LoginResponse> LoginAsync(LoginByPasswordRequest request)
        {
            if (request == null)
                return new LoginResponse { Success = false, Message = "Request body is required." };

            var code = (request.Password ?? "").Trim();

            if (code.Length != 5 || !code.All(char.IsDigit))
                return new LoginResponse { Success = false, Message = "Password must be exactly 5 digits." };

            var activeUsers = await _userRepo.GetActiveUsersAsync();
            if (activeUsers == null || activeUsers.Count == 0)
                return new LoginResponse { Success = false, Message = "No active users found." };

            User matchedUser = null;

            // Find first user whose hash matches this 6-digit code
            foreach (var user in activeUsers)
            {
                if (user == null) continue;

                if (_passwordService.Verify(user, code))
                {
                    matchedUser = user;
                    break;
                }
            }

            if (matchedUser == null)
                return new LoginResponse { Success = false, Message = "Invalid password." };

            var token = _tokenService.GenerateToken(matchedUser);

            return new LoginResponse
            {
                Success = true,
                Message = "Login successful.",
                Token = token,
                User = new UserDto
                {
                    UserId = matchedUser.UserId,
                    Username = matchedUser.UserName,
                    FullNameEn = matchedUser.FullNameEn,
                    FullNameAr = matchedUser.FullNameAr,
                    RoleId = matchedUser.RoleId,
                    IsActive = matchedUser.IsActive
                }
            };
        }
    }
}
