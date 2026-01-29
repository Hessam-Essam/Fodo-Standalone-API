using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fodo.Domain.Entities
{
    [Table("Branches")]
    public class Branches
    {
        [Key]
        [Column("BranchId")]
        public int BranchId { get; set; }

        [Column("Code")]
        public string Code { get; set; }

        [Column("NameEn")]
        public string NameEn { get; set; }

        [Column("NameAr")]
        public string NameAr { get; set; }

        [Column("Address")]
        public string Address { get; set; }

        [Column("Phone")]
        public string Phone { get; set; }

        [Column("IsActive")]
        public bool IsActive { get; set; }

        [ForeignKey(nameof(Clients))]
        [Column("client_id")]
        public int ClientId { get; set; }

        public Clients Clients { get; set; }
    }
}
