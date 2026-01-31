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
        public int? TaxRuleId { get; set; }
        public int ClientId { get; set; }
    }
}
