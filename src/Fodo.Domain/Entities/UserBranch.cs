using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Domain.Entities
{
    public class UserBranch
    {
        public Guid UserId { get; set; }
        public Guid BranchId { get; set; }
    }

}
