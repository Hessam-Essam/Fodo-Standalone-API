using Fodo.Contracts.DTOS;
using Fodo.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Application.Implementation.Interfaces
{
    public interface IDeviceVerificationService
    {
        Task<VerifyDeviceResponse> VerifyAsync(DevicesDto request);
    }
}
