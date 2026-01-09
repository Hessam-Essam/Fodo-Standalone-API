using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Application.Common.Execptions
{
    public sealed class UnauthorizedException : Exception
    {
        public UnauthorizedException()
            : base("Unauthorized access.")
        {
        }

        public UnauthorizedException(string message)
            : base(message)
        {
        }

        public UnauthorizedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }

}
