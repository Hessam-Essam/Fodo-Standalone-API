using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Application.Features.Login
{
    public sealed class LoginUserDto
    {
        public Guid Id { get; init; }
        public string Username { get; init; } = default!;
        public string FullNameAr { get; set; }
        public string FullNameEn { get; set; }
        public bool IsActive { get; set; }
        public Guid RoleId { get; set; }
        public string Role { get; init; } = default!;
        public int ClientId { get; init; } = default!;

    }

}
