using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Contracts.DTOS
{
    public class ItemsDto
    {
        public int ItemId { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public int ItemType { get; set; }
        public int CategoryId { get; set; }
        public decimal BasePrice { get; set; }
        public bool IsActive { get; set; }
        public TaxRulesDto TaxRule { get; set; }

        public List<ItemPricesDto> Prices { get; set; }
    }
}
