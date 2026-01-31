using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fodo.Domain.Entities
{
    public class UserBranches
    {
        [Key]
        public int UserBranchId { get; set; }
        [ForeignKey(nameof(User))]
        [Column("UserId")]
        public Guid UserId { get; set; }

        [ForeignKey(nameof(Branch))]
        [Column("BranchId")]
        public int BranchId { get; set; }

        public User User { get; set; }
        public Branches Branch { get; set; }
    }

}
