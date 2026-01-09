using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Application.Features.Login
{
    public sealed class LoginUserDto
    {
        public Guid Id { get; init; }
        public string Username { get; init; } = default!;
        public string Role { get; init; } = default!;
    }

}
