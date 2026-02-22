using Fodo.Application.Features.Cash.CashIn.CreateCashIn;
using Fodo.Application.Features.Cash.CashIn.GetAllCashIn;
using Fodo.Application.Features.Cash.CashOut.CreateCashOut;
using Fodo.Application.Features.Cash.CashOut.GetAllCashOut;
using Fodo.Application.Implementation.Interfaces;
using Fodo.Application.Implementation.IRepositories;
using Fodo.Contracts.DTOS;
using Fodo.Domain.Entities;

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

        public async Task<GetAllCashInResponse> GetAllAsync(GetAllCashInRequest request, CancellationToken ct)
        {
            if (request is null)
                return new(false, "Request body is required.", Array.Empty<CashInVoucherDto>());

            if (request.BranchId <= 0)
                return new(false, "BranchId is required.", Array.Empty<CashInVoucherDto>());

            if (request.ShiftId <= 0)
                return new(false, "ShiftId is required.", Array.Empty<CashInVoucherDto>());

            var shift = await _repo.GetShiftAsync(request.ShiftId, ct);
            if (shift is null)
                return new(false, "Shift not found.", Array.Empty<CashInVoucherDto>());

            if (shift.BranchId != request.BranchId)
                return new(false, "Shift does not belong to this branch.", Array.Empty<CashInVoucherDto>());

            var rows = await _repo.GetAllByShiftAsync(request.BranchId, request.ShiftId, request.DeviceId, ct);

            var data = rows.Select(x => new CashInVoucherDto(
                CashInVoucherId: x.CashInVoucherId,
                BranchId: x.BranchId,
                ShiftId: x.ShiftId,
                UserId: x.UserId,
                Amount: Math.Round(x.Amount, 2, MidpointRounding.AwayFromZero),
                Reason: x.Reason,
                CreatedAtUtc: x.CreatedAt,
                DeviceId: x.DeviceId,
                RowGuid: x.RowGuid,
                IsSynced: x.IsSynced,
                SyncedAtUtc: x.SyncedAt
            )).ToList();

            return new(true, "Cash-In vouchers loaded successfully.", data);
        }
        public async Task<GetAllCashOutResponse> GetAllAsync(GetAllCashOutRequest request, CancellationToken ct)
        {
            if (request is null)
                return new(false, "Request body is required.", Array.Empty<CashOutVoucherDto>());

            if (request.BranchId <= 0)
                return new(false, "BranchId is required.", Array.Empty<CashOutVoucherDto>());

            if (request.ShiftId <= 0)
                return new(false, "ShiftId is required.", Array.Empty<CashOutVoucherDto>());

            var shift = await _repo.GetShiftAsync(request.ShiftId, ct);
            if (shift is null)
                return new(false, "Shift not found.", Array.Empty<CashOutVoucherDto>());

            if (shift.BranchId != request.BranchId)
                return new(false, "Shift does not belong to this branch.", Array.Empty<CashOutVoucherDto>());

            var rows = await _repo.GetAllCashOutByShiftAsync(request.BranchId, request.ShiftId, request.DeviceId, ct);

            var data = rows.Select(x => new CashOutVoucherDto(
                CashOutVoucherId: x.CashOutVoucherId,
                BranchId: x.BranchId,
                ShiftId: x.ShiftId,
                UserId: x.UserId,
                Amount: Math.Round(x.Amount, 2, MidpointRounding.AwayFromZero),
                Reason: x.Reason,
                CreatedAtUtc: x.CreatedAt,
                DeviceId: x.DeviceId,
                RowGuid: x.RowGuid,
                IsSynced: x.IsSynced,
                SyncedAtUtc: x.SyncedAt
            )).ToList();

            return new(true, "Cash-Out vouchers loaded successfully.", data);
        }
    }
}
