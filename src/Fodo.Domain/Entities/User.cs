using Fodo.Domain.Common;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Fodo.Domain.Entities
{
    public class User : IdentityUser
    {
        public Guid UserId { get; set; }

        public string FullNameAr { get; set; }
        public string FullNameEn { get; set; }

        public string PasswordHash { get; set; }   // 6-digit hashed

        public bool IsActive { get; set; }

        public Guid RoleId { get; set; }
        public Role Role { get; set; }

        public ICollection<UserBranch> UserBranches { get; set; }
    }

}
