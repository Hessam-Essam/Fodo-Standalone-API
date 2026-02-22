using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fodo.Domain.Entities
{
    public class OrderItemModifier
    {
        [Key]
        public long OrderItemModifierId { get; set; }

        [ForeignKey(nameof(OrderItems))]
        [Column("OrderItemId")]
        public long OrderItemId { get; set; }

        [ForeignKey(nameof(ModifiersGroup))]
        [Column("ModifierGroupId")]
        public int ModifierGroupId { get; set; }

        
        public int ModifierId { get; set; }
        public decimal UnitPrice { get; set; }

        public OrderItems OrderItems { get; set; } = null!;
        public ModifiersGroup ModifiersGroup { get; set; } = null!;

        [NotMapped]
        public OrderItems? _tempOrderItem { get; set; }
    }
}
