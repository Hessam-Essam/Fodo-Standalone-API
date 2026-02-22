using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Domain.Entities
{
    public sealed class OrderRefund
    {
        public long RefundId { get; set; }

        public long OrderId { get; set; }
        public int BranchId { get; set; }
        public int ShiftId { get; set; }
        public Guid? UserId { get; set; }

        public decimal RefundAmount { get; set; }
        public string? Reason { get; set; }

        public DateTime CreatedAt { get; set; }

        public Guid RowGuid { get; set; }
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();

        public string DeviceId { get; set; } = null!;
        public DateTime? SyncedAt { get; set; }
        public bool IsSynced { get; set; }
    }
}
