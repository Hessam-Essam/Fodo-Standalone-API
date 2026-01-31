using Fodo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Application.Implementation.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
