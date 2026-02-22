using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fodo.Domain.Entities
{
    public class ModifiersGroup
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Clients))]
        [Column("client_id")]
        public int ClientId { get; set; }

        public string NameEn { get; set; } = null!;
        public string? NameAr { get; set; }

        public int MinSelect { get; set; } = 0;
        public int MaxSelect { get; set; } = 1;
        public bool IsRequired { get; set; } = false;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Clients Clients { get; set; }
        public ICollection<Modifiers> Modifiers { get; set; } = new List<Modifiers>();
        public ICollection<ItemsModifierGroup> ItemsModifierGroup { get; set; } = new List<ItemsModifierGroup>();
    }
}
