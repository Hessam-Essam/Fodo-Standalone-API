using Fodo.Contracts.DTOS;
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
        public string Message { get; set; }
        public string Token { get; set; }
        public UserDto User { get; set; }

        public static LoginResponse Fail(string error)
            => new() { Success = false, Error = error };

        public static LoginResponse Ok(
            UserDto user)
            => new()
            {
                Success = true,
                User = user
            };
    }

}
