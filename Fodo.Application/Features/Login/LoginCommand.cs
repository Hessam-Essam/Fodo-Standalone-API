using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Application.Features.Login
{
    public sealed record LoginCommand(
        string Username,
        string Password,
        string DeviceId
    ) : IRequest<LoginResponse>;

}
