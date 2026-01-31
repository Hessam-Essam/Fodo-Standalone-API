using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Contracts.DTOS
{
    public class TaxRulesDto
    {
        public int TaxRuleId { get; set; }
        public string NameAR { get; set; }
        public string NameEN { get; set; }
        public decimal Rate { get; set; }
        public int ClientId { get; set; }
    }
}
