using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fodo.Domain.Entities
{
    public class TaxRules
    {
        [Key]
        public int TaxRuleId { get; set; }
        public string NameAR { get; set; }
        public string NameEN { get; set; }
        public decimal Rate { get; set; }

        [ForeignKey(nameof(Clients))]
        [Column("client_id")]
        public int ClientId { get; set; }

        public Clients Clients { get; set; }

        public ICollection<Items> Items { get; set; }
    }
}
