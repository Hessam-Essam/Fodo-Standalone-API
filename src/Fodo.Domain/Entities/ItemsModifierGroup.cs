using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fodo.Domain.Entities
{
    public sealed class ItemsModifierGroup
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Item))]
        [Column("item_id")]
        public int ItemId { get; set; }
        public int ModifierGroupId { get; set; }

        [ForeignKey(nameof(Clients))]
        [Column("client_id")]
        public int ClientId { get; set; }

        public int SortOrder { get; set; } = 0;
        public bool IsActive { get; set; } = true;

        public Items Item { get; set; } = null!;
        public ModifiersGroup ModifiersGroup { get; set; } = null!;
        public Clients Clients { get; set; }
    }
}
