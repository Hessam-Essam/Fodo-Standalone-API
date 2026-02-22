using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fodo.Domain.Entities
{
    public sealed class Payments
    {
        [Key]
        public long PaymentId { get; set; }
        [ForeignKey(nameof(Order))]
        [Column("OrderId")]
        public long OrderId { get; set; }

        [ForeignKey(nameof(PaymentMethod))]
        [Column("PaymentMethodId")]
        public int? PaymentMethodId { get; set; }
        public string? Method { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid RowGuid { get; set; }

        public byte[] RowVersion { get; set; } = Array.Empty<byte>();

        public string DeviceId { get; set; } = null!;
        public DateTime? SyncedAt { get; set; }
        public bool IsSynced { get; set; }

        // Navigation
        public Payment_Methods? PaymentMethod { get; set; }
        public Orders Order { get; set; } = null!;
       
    }
}
