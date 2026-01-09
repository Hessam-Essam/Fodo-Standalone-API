using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Application.Features.Login
{
    public sealed class LoginResponse
    {
        public bool Success { get; init; }
        public string? Error { get; init; }
        public string? AccessToken { get; init; }
        public int ExpiresIn { get; init; }

        public LoginUserDto? User { get; init; }

        public static LoginResponse Fail(string error)
            => new() { Success = false, Error = error };

        public static LoginResponse Ok(
            string token,
            int expiresIn,
            LoginUserDto user)
            => new()
            {
                Success = true,
                AccessToken = token,
                ExpiresIn = expiresIn,
                User = user
            };
    }

}
