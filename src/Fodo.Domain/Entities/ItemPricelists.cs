using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Fodo.Domain.Entities
{
    public class ItemPricelists
    {
        [Key]
        public int ItemId { get; set; }
        public int PricelistId { get; set; }
        public decimal Price { get; set; }

        public Items Items { get; set; }
        public PriceLists PriceList { get; set; }
    }

}
