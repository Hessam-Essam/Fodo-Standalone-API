using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fodo.Domain.Entities
{
    public class Items
    {
        [Key]
        public int ItemId { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public int ItemType { get; set; }
        public int CategoryId { get; set; }
        public decimal BasePrice { get; set; }
        public bool IsActive { get; set; }
        public int? TaxRuleId { get; set; }

        [ForeignKey(nameof(Clients))]
        [Column("client_id")]
        public int ClientId { get; set; }

        public Clients Clients { get; set; }

        public Categories Category { get; set; }
        public TaxRules TaxRule { get; set; }
        public ICollection<ItemPricelists> ItemPricelists { get; set; }
    }
}
