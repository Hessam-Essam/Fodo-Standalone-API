using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Fodo.Domain.Entities
{
    public class CategoryBranches
    {
        [Key]
        public int CategoryId { get; set; }
        public int BranchId { get; set; }

        public Categories Category { get; set; }
        public Branches Branches { get; set; }
    }
}
