using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Contracts.DTOS
{
    public class ItemPricelistsDto
    {
        public int ItemId { get; set; }
        public int PricelistId { get; set; }
        public decimal Price { get; set; }
    }
}
