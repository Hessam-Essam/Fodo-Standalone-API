using Fodo.Application.Features.Cash.CashIn;
using Fodo.Application.Features.Cash.CashOut;
using Fodo.Application.Implementation.Interfaces;
using Fodo.Application.Implementation.IRepositories;
using Fodo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fodo.Application.Implementation.Services
{
    public sealed class CashService : ICashService
    {
        private readonly ICashRepository _repo;
        private readonly IUnitOfWork _uow;

        public CashService(ICashRepository repo, IUnitOfWork uow)
        {
            _repo = repo;
            _uow = uow;
        }

        public async Task<CreateCashInResponse> CreateAsync(CreateCashInRequest request, CancellationToken ct)
        {
            if (request is null)
                return new(false, "Request body is required.", null, null, null);

            if (request.BranchId <= 0)
                return new(false, "BranchId is required.", null, null, null);

            if (request.ShiftId <= 0 || request.ShiftId == null)
                return new(false, "ShiftId is required.", null, null, null);

            if (string.IsNullOrWhiteSpace(request.DeviceId))
                return new(false, "DeviceId is required.", null, null, null);

            if (request.Amount <= 0)
                return new(false, "Amount must be > 0.", null, null, null);

            // Validate shift
            var shift = await _repo.GetShiftAsync(request.ShiftId, ct);
            if (shift is null)
                return new(false, "Shift not found.", null, null, null);

            if (shift.BranchId != request.BranchId)
                return new(false, "Shift does not belong to this branch.", null, null, null);

            // Optional: reject if shift already ended
            if (shift.EndTime != null)
                return new(false, "Shift already closed.", null, null, null);

            var now = DateTime.UtcNow;

            var entity = new CashInVoucher
            {
                BranchId = request.BranchId,
                ShiftId = request.ShiftId,
                UserId = request.UserId,

                Amount = Math.Round(request.Amount, 2, MidpointRounding.AwayFromZero),
                Reason = request.Reason,

                CreatedAt = now,

                RowGuid = Guid.NewGuid(),
                DeviceId = request.DeviceId,
                SyncedAt = null,
                IsSynced = false
            };

            // Begin transaction (BeginAsync returns Task according to provided IUnitOfWork signature).
            await _uow.BeginAsync(ct);

            try
            {
                await _repo.AddAsync(entity, ct);
                await _uow.SaveChangesAsync(ct);

                // Commit using the unit of work's CommitAsync method.
                await _uow.CommitAsync(ct);
            }
            catch
            {
                // Attempt to rollback; swallow any rollback exceptions to not hide the original exception.
                try
                {
                    await _uow.RollbackAsync(ct);
                }
                catch
                {
                    // ignored
                }
                throw;
            }

            return new(true, "Cash-In voucher created successfully.", entity.CashInVoucherId, entity.RowGuid, entity.CreatedAt);
        }

        public async Task<CreateCashOutResponse> CreateAsync(CreateCashOutRequest request, CancellationToken ct)
        {
            if (request is null)
                return new(false, "Request body is required.", null, null, null);

            if (request.BranchId <= 0)
                return new(false, "BranchId is required.", null, null, null);

            if (request.ShiftId <= 0)
                return new(false, "ShiftId is required.", null, null, null);

            if (string.IsNullOrWhiteSpace(request.DeviceId))
                return new(false, "DeviceId is required.", null, null, null);

            if (request.Amount <= 0)
                return new(false, "Amount must be > 0.", null, null, null);

            var shift = await _repo.GetShiftAsync(request.ShiftId, ct);
            if (shift is null)
                return new(false, "Shift not found.", null, null, null);

            if (shift.BranchId != request.BranchId)
                return new(false, "Shift does not belong to this branch.", null, null, null);

            if (shift.EndTime != null)
                return new(false, "Shift already closed.", null, null, null);

            var now = DateTime.UtcNow;

            var entity = new CashOutVoucher
            {
                BranchId = request.BranchId,
                ShiftId = request.ShiftId,
                UserId = request.UserId,

                Amount = Math.Round(request.Amount, 2, MidpointRounding.AwayFromZero),
                Reason = request.Reason,

                CreatedAt = now,

                RowGuid = Guid.NewGuid(),
                DeviceId = request.DeviceId,
                SyncedAt = null,
                IsSynced = false
            };

            // Begin transaction: call BeginAsync without assigning its result because it returns Task.
            await _uow.BeginAsync(ct);

            try
            {
                await _repo.AddAsync(entity, ct);
                await _uow.SaveChangesAsync(ct);

                // Commit with the unit of work
                await _uow.CommitAsync(ct);
            }
            catch
            {
                // Rollback on error; swallow rollback exceptions.
                try
                {
                    await _uow.RollbackAsync(ct);
                }
                catch
                {
                    // ignored
                }
                throw;
            }

            return new(true, "Cash-Out voucher created successfully.", entity.CashOutVoucherId, entity.RowGuid, entity.CreatedAt);
        }
    }
}
