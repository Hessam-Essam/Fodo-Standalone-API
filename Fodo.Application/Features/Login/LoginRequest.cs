using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Application.Features.Login
{
    public sealed class LoginRequest
    {
        public string Username { get; init; } = default!;
        public string Password { get; init; } = default!;
        public string DeviceId { get; init; } = default!;
    }

}
