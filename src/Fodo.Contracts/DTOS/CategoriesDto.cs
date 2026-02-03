using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Contracts.DTOS
{
    public class CategoriesDto
    {
        public int CategoryId { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public bool IsActive { get; set; }
        public int ClientId { get; set; }

        public List<ItemsDto> Items { get; set; }
    }
}
