using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fodo.Domain.Entities
{
    public sealed class Orders
    {
        [Key]
        public long OrderId { get; set; }
        public string OrderNumber { get; set; } = null!;

        [ForeignKey(nameof(Branch))]
        [Column("BranchId")]
        public int BranchId { get; set; }

        public Guid? UserId { get; set; }

        [ForeignKey(nameof(Shifts))]
        [Column("ShiftId")]
        public int? ShiftId { get; set; }
        public string Status { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid RowGuid { get; set; }
        public string DeviceId { get; set; } = null!;
        public DateTime? SyncedAt { get; set; }
        public bool IsSynced { get; set; }
        public string? Note { get; set; }
        public Branches Branch { get; set; } = null!;
        public Shifts Shifts { get; set; } = null!;
        public ICollection<OrderItems> OrderItems { get; set; } = new List<OrderItems>();
    }
}
