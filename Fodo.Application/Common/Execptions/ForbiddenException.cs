using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Application.Common.Execptions
{
    public sealed class ForbiddenException : Exception
    {
        public ForbiddenException()
            : base("Access is forbidden.")
        {
        }

        public ForbiddenException(string message)
            : base(message)
        {
        }

        public ForbiddenException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
