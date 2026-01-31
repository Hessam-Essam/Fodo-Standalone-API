using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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

        [ForeignKey(nameof(Clients))]
        [Column("client_id")]
        public int? ClientId { get; set; }

        public Clients Clients { get; set; }
        public ICollection<RolePermission> Permissions { get; set; }
    }

}
