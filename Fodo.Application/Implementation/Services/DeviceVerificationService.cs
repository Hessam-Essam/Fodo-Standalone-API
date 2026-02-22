using Fodo.Application.Handlers;
using Fodo.Application.Implementation.Interfaces;
using Fodo.Application.Implementation.IRepositories;
using Fodo.Contracts.DTOS;
using Fodo.Contracts.Responses;
using Fodo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Application.Implementation.Services
{
    public class DeviceVerificationService : IDeviceVerificationService
    {
        private readonly IDeviceRepository _deviceRepository;

        public DeviceVerificationService(IDeviceRepository deviceRepository)
        {
            _deviceRepository = deviceRepository;
        }

        public async Task<VerifyDeviceResponse> VerifyAsync(DevicesDto request)
        {
            if (request == null)
                return new VerifyDeviceResponse { Success = false, Message = "Request body is required." };

            if (request.BranchId <= 0)
                return new VerifyDeviceResponse { Success = false, Message = "BranchId is required." };

            var mac = MacAddressHelper.Normalize(request.MacAddress);
            if (string.IsNullOrEmpty(mac))
                return new VerifyDeviceResponse { Success = false, Message = "MacAddress is required." };

            var device = await _deviceRepository.GetByBranchAndMacAsync(request.BranchId, mac);

            if (device == null)
                return new VerifyDeviceResponse
                {
                    Success = false,
                    Message = "Device not found."
                };

            return new VerifyDeviceResponse
            {
                Success = true,
                Message = "Successfully found.",
                DeviceId = device.DeviceCode
            };
        }

    }
}
