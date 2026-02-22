using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fodo.Domain.Entities
{
    public sealed class Shifts
    {
        [Key]
        public int ShiftId { get; set; }

        [ForeignKey(nameof(Branch))]
        [Column("BranchId")]
        public int BranchId { get; set; }

        [ForeignKey(nameof(User))]
        [Column("UserId")]
        public Guid UserId { get; set; }                

        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }          

        public decimal OpeningCash { get; set; }
        public decimal? ClosingCash { get; set; }        

        // Navigation properties
        public Branches Branch { get; set; } = null!;
        public User User { get; set; } = null!;

        // Optional: Orders placed in this shift
        public ICollection<Orders> Orders { get; set; } = new List<Orders>();
    }
}
