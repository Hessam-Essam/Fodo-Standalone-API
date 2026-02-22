using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fodo.Domain.Entities
{
    public sealed class Payment_Methods
    {
        [Key]
        public int PaymentMethodId { get; set; }

        [ForeignKey(nameof(Clients))]
        [Column("client_id")]
        public int ClientId { get; set; }
        public string MethodName { get; set; } = null!;
        public string MethodType { get; set; } = null!;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        // Optional navigation if you want it (not required)
        public ICollection<Payments> Payments { get; set; } = new List<Payments>();

        public Clients Clients { get; set; }
    }
}
