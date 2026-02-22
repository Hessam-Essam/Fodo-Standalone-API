using Fodo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Application.Implementation.IRepositories
{
    public interface IDeviceRepository
    {
        Task<bool> DeviceExistsAsync(int branchId, string normalizedMac);
        Task<Devices?> GetByBranchAndMacAsync(int branchId, string mac);

    }
}
