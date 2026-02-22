using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fodo.Domain.Entities
{
    public sealed class ModifiersPricelist
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Modifiers))]
        [Column("ModifierId")]
        public int ModifierId { get; set; }
        [ForeignKey(nameof(PriceLists))]
        [Column("PriceListId")]
        public int PriceListId { get; set; }

        public decimal Price { get; set; }

        public Modifiers Modifiers { get; set; } = null!;
        public PriceLists PriceLists { get; set; } = null!;
    }
}
