using Fodo.Application.Implementation.IRepositories;
using Fodo.Domain.Entities;
using Fodo.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Infrastructure.Repositories
{
    public class DeviceRepository : IDeviceRepository
    {
        private readonly IdentityDbContext _db;

        public DeviceRepository(IdentityDbContext db)
        {
            _db = db;
        }

        public Task<bool> DeviceExistsAsync(int branchId, string normalizedMac)
        {
            return _db.Devices.AnyAsync(d =>
                d.BranchId == branchId &&
                d.MacAddress == normalizedMac);
        }

        public async Task<Devices?> GetByBranchAndMacAsync(int branchId, string mac)
        {
            return await _db.Devices
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.BranchId == branchId && d.MacAddress == mac);
        }
    }
}
