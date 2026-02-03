using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Contracts.DTOS
{
    public class ItemPricesDto
    {
        public int PricelistId { get; set; }
        public string PriceListNameEn { get; set; }
        public string PriceListNameAr { get; set; }
        public decimal Price { get; set; }
    }
}
