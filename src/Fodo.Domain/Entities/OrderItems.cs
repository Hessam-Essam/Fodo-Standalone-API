using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fodo.Domain.Entities
{
    public class OrderItems
    {
        [Key]
        public long OrderItemId { get; set; }

        [ForeignKey(nameof(Order))]
        [Column("OrderId")]
        public long OrderId { get; set; }

        [ForeignKey(nameof(Items))]
        [Column("ItemId")]
        public int ItemId { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }
        public string? Note { get; set; }
        public Orders Order { get; set; } = null!;
        public Items Items { get; set; } = null!;
        public ICollection<OrderItemModifier> Modifiers { get; set; } = new List<OrderItemModifier>();
    }
}
