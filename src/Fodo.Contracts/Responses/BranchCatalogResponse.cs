using Fodo.Contracts.DTOS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Contracts.Responses
{
    public class BranchCatalogResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public BranchesDto Branch { get; set; }
        public List<CategoriesDto> Categories { get; set; }
        public List<CategoryBranchesDto> CategoryBranches { get; set; }
        public List<ItemsDto> Items { get; set; }
        public List<ItemPricelistsDto> ItemPricelists { get; set; }
        public List<PriceListsDto> PriceLists { get; set; }
        public List<TaxRulesDto> TaxRules { get; set; }
    }
}
