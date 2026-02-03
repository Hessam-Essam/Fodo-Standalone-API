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

        public MenuDto Menu { get; set; }
    }
}
