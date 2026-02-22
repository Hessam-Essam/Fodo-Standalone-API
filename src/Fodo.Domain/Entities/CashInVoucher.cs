using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fodo.Domain.Entities
{
    public sealed class CashInVoucher
    {
        public long CashInVoucherId { get; set; }

        [ForeignKey(nameof(Branches))]
        [Column("BranchId")]
        public int BranchId { get; set; }

        [ForeignKey(nameof(Shifts))]
        [Column("ShiftId")]
        public int ShiftId { get; set; }

        [ForeignKey(nameof(Users))]
        [Column("UserId")]
        public Guid? UserId { get; set; }

        public decimal Amount { get; set; }
        public string? Reason { get; set; }

        public DateTime CreatedAt { get; set; }

        public Guid RowGuid { get; set; }
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();

        public string DeviceId { get; set; } = null!;
        public DateTime? SyncedAt { get; set; }
        public bool IsSynced { get; set; }

        public Branches Branches { get; set; }
        public Shifts Shifts { get; set; }
        public User Users { get; set; }
    }
}
