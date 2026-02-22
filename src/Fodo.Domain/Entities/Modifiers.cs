using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fodo.Domain.Entities
{
    public sealed class Modifiers
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(ModifiersGroup))]
        [Column("ModifierGroupId")]
        public int ModifierGroupId { get; set; }

        [ForeignKey(nameof(Clients))]
        [Column("client_id")]
        public int ClientId { get; set; }

        public string NameEn { get; set; } = null!;
        public string? NameAr { get; set; }

        public decimal BasePrice { get; set; } = 0m;

        public bool IsActive { get; set; } = true;
        public int SortOrder { get; set; } = 0;

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Clients Clients { get; set; }
        public ModifiersGroup ModifiersGroup { get; set; } = null!;
        public ICollection<ModifiersPricelist> ModifiersPricelist { get; set; } = new List<ModifiersPricelist>();
    }
}
