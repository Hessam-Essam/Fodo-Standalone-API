using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fodo.Domain.Entities
{
    [Table("Devices")]
    public class Devices
    {
        [Key]
        public int DeviceId { get; set; }

        [ForeignKey(nameof(Branches))]
        [Column("BranchId")]
        public int BranchId { get; set; }

        [Required]
        [MaxLength(50)]
        public string DeviceCode { get; set; } = string.Empty;

        public bool IsMain { get; set; }

        [MaxLength(100)]
        public string? MacAddress { get; set; }

        public DateTime? LastSyncAt { get; set; }

        public Branches Branches { get; set; }
    }
}
