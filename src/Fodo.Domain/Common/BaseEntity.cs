using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Domain.Common
{
    public abstract class BaseEntity
    {
        public DateTime CreatedAt { get; protected set; }
        public DateTime? UpdatedAt { get; protected set; }
        public Guid RowGuid { get; protected set; }
        public bool IsDeleted { get; protected set; }
    }
}
