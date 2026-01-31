using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Contracts.DTOS
{
    public class PriceListsDto
    {
        public int PriceListId { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public bool IsActive { get; set; }
        public int ClientId { get; set; }
    }
}
