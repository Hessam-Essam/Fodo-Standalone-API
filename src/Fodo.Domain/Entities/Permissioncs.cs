using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Domain.Entities
{
    public class Permission
    {
        public Guid Id { get; set; }

        public string Code { get; set; } // e.g. ORDER_REFUND
        public string Module { get; set; } // OrderFlow, Inventory
    }

}
