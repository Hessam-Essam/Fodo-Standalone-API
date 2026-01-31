using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fodo.Domain.Entities
{
    public class Categories
    {
        [Key]
        public int CategoryId { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public bool IsActive { get; set; }

        [ForeignKey(nameof(Clients))]
        [Column("client_id")]
        public int ClientId { get; set; }

        public Clients Clients { get; set; }

        public ICollection<CategoryBranches> CategoryBranches { get; set; }
        public ICollection<Items> Items { get; set; }
    }
}
