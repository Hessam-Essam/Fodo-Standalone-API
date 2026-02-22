using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Contracts.DTOS
{
    public class ModifiersDto
    {
        public int ModifierId { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public decimal BasePrice { get; set; }
        public bool IsActive { get; set; }
        public int Sequence { get; set; } // comes from Modifiers.SortOrder
        public List<ModifiersPricesDto> Prices { get; set; } = new();
    }
}
