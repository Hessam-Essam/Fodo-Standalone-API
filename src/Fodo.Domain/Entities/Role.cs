using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Domain.Entities
{
    public class Role
    {
        public Guid Id { get; set; }

        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string Description { get; set; }

        public bool IsActive { get; set; }

        public ICollection<RolePermission> Permissions { get; set; }
    }

}
