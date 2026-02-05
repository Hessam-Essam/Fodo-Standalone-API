using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Contracts.DTOS
{
    public class UserDto
    {
        public Guid UserId { get; set; }
        public string? Username { get; set; }
        public string? FullNameEn { get; set; }
        public string? FullNameAr { get; set; }
        public Guid RoleId { get; set; }
        public bool IsActive { get; set; }
        public int ClientId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
