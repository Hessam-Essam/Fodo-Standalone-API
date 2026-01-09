using Fodo.Domain.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Fodo.Domain.Entities
{
    public class User : BaseEntity
    {
        public Guid Id { get; set; }

        public string FullNameAr { get; set; }
        public string FullNameEn { get; set; }

        public string Username { get; set; }
        public string PasswordHash { get; set; }   // 6-digit hashed

        public bool IsActive { get; set; }

        public Guid RoleId { get; set; }
        public Role Role { get; set; }

        public ICollection<UserBranch> UserBranches { get; set; }
    }

}
