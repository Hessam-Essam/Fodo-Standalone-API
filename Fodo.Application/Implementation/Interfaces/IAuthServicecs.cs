using Fodo.Application.Features.Login;
using Fodo.Contracts.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Application.Implementation.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponse> LoginAsync(LoginByPasswordRequest request);
    }

}
