using Fodo.Application.Features.Shifts;
using Fodo.Application.Features.Shifts.CloseShift;
using Fodo.Application.Features.Shifts.EndDay;
using Fodo.Application.Features.Shifts.StartShift;
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
    public class ShiftService : IShiftService
    {
        private readonly IShiftRepository _repo;
        private readonly IOrdersRepository _orderRepo;
        private readonly IMenuRepository _menuRepo;
        private readonly IPaymentsRepository _payRepo;
        private readonly ICashRepository _cashRepo;
        private readonly IUnitOfWork _uow;

        public ShiftService(IShiftRepository repo, ICashRepository cashRepo, IOrdersRepository orderRepo, IMenuRepository menuRepo, IPaymentsRepository payRepo, IUnitOfWork uow)
        {
            _repo = repo;
            _uow = uow;
            _orderRepo = orderRepo;
            _menuRepo = menuRepo;
            _cashRepo = cashRepo;
            _payRepo = payRepo;
        }

        public async Task<StartShiftResponse> StartShiftAsync(StartShiftRequest request, CancellationToken ct)
        {
            if (request.BranchId <= 0)
                return new StartShiftResponse(false, "branchId is required.", null);

            if (request.UserId == Guid.Empty)
                return new StartShiftResponse(false, "userId is required.", null);

            if (request.OpeningCash < 0)
                return new StartShiftResponse(false, "openingCash cannot be negative.", null);

            // Check open shift
            var open = await _repo.GetOpenShiftAsync(request.BranchId, request.UserId, ct);
            if (open != null)
                return new StartShiftResponse(true, "Shift already open.", open.ShiftId);

            var startTime = request.StartTimeUtc ?? DateTime.UtcNow;

            var shift = new Shifts
            {
                BranchId = request.BranchId,
                UserId = request.UserId,
                StartTime = startTime,
                EndTime = null,
                OpeningCash = Math.Round(request.OpeningCash, 2, MidpointRounding.AwayFromZero),
                ClosingCash = null
            };

            // Begin unit of work (BeginAsync returns Task, not a transaction object)
            await _uow.BeginAsync(ct);

            try
            {
                _ = await _repo.CreateAsync(shift, ct);

                await _uow.SaveChangesAsync(ct);
                await _uow.CommitAsync(ct);
            }
            catch
            {
                // Attempt rollback, but don't hide the original exception if rollback fails
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

            return new StartShiftResponse(true, "Shift started successfully.", shift.ShiftId);
        }


        public async Task<CloseShiftResponse> CloseShiftAsync(CloseShiftRequest request, CancellationToken ct)
        {
            /* PSEUDOCODE (detailed plan):
               - Validate request parameters (shift id, branch id, device id, user id, closing cash non-negative)
               - Load the shift by id from repository
               - Validate that shift exists, belongs to the provided branch and user, and is not already closed
               - Set EndTime to UtcNow and set ClosingCash (rounded)
               - Begin unit of work by awaiting _uow.BeginAsync(ct)
               - In a try block:
                   - Update the shift via repository: await _repo.UpdateAsync(shift, ct)
                   - Persist changes: await _uow.SaveChangesAsync(ct)
                   - Commit the unit of work: await _uow.CommitAsync(ct)
               - In catch:
                   - Attempt rollback: await _uow.RollbackAsync(ct) inside its own try/catch to avoid hiding original exception
                   - Rethrow the original exception
               - Return successful response when done
            */

            if (request.ShiftId <= 0)
                return new CloseShiftResponse(false, "shiftId is required.");

            if (request.BranchId <= 0)
                return new CloseShiftResponse(false, "branchId is required.");

            if (string.IsNullOrWhiteSpace(request.DeviceId))
                return new CloseShiftResponse(false, "deviceId is required.");

            if (request.UserId == Guid.Empty)
                return new CloseShiftResponse(false, "userId is required.");

            if (request.ClosingCash < 0)
                return new CloseShiftResponse(false, "closingCash cannot be negative.");

            var shift = await _repo.GetByIdAsync(request.ShiftId, ct);

            if (shift == null)
                return new CloseShiftResponse(false, "Shift not found.");

            if (shift.BranchId != request.BranchId)
                return new CloseShiftResponse(false, "Shift does not belong to this branch.");

            if (shift.UserId != request.UserId)
                return new CloseShiftResponse(false, "Shift does not belong to this user.");

            if (shift.EndTime != null)
                return new CloseShiftResponse(false, "Shift already closed.");

            shift.EndTime = DateTime.UtcNow;
            shift.ClosingCash = Math.Round(request.ClosingCash, 2, MidpointRounding.AwayFromZero);

            // Begin the unit of work. Do not assign its result to a variable because BeginAsync returns Task.
            await _uow.BeginAsync(ct);

            try
            {
                // Use the async repository method that exists on IShiftRepository
                await _repo.UpdateAsync(shift, ct);
                await _uow.SaveChangesAsync(ct);
                await _uow.CommitAsync(ct);
            }
            catch
            {
                // Attempt rollback, but don't swallow the original exception if rollback fails
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

            return new CloseShiftResponse(true, "Shift closed successfully.");
        }

        public async Task<ShiftVisaTotalResponse> GetVisaTotalAsync(
            ShiftVisaTotalRequest request,
            CancellationToken ct)
        {
            if (request.BranchId <= 0)
                return new(false, "BranchId is required.", request.BranchId, request.ShiftId, default, default, 0m);

            if (request.ShiftId <= 0)
                return new(false, "ShiftId is required.", request.BranchId, request.ShiftId, default, default, 0m);

            var branch = await _menuRepo.GetBranchAsync(request.BranchId, ct);
            if (branch is null)
                return new(false, "Branch not found.", request.BranchId, request.ShiftId, default, default, 0m);

            var shift = await _repo.GetByIdAsync(request.ShiftId, ct);
            if (shift is null)
                return new(false, "Shift not found.", request.BranchId, request.ShiftId, default, default, 0m);

            if (shift.BranchId != request.BranchId)
                return new(false, "Shift does not belong to this branch.", request.BranchId, request.ShiftId, default, default, 0m);

            // 1️⃣ Get VISA method IDs
            var visaMethodIds = await _payRepo.GetVisaPaymentMethodIdsAsync(branch.ClientId, ct);
            if (visaMethodIds.Count == 0)
                return new(true, "No VISA methods found.", request.BranchId, request.ShiftId,
                    shift.StartTime,
                    shift.EndTime ?? DateTime.UtcNow,
                    0m);

            // 2️⃣ Get Orders for shift
            var orderIds = await _orderRepo.GetOrderIdsByShiftAsync(request.BranchId, request.ShiftId, ct);
            if (orderIds.Count == 0)
                return new(true, "No orders in this shift.", request.BranchId, request.ShiftId,
                    shift.StartTime,
                    shift.EndTime ?? DateTime.UtcNow,
                    0m);

            // 3️⃣ Sum VISA payments for those orders
            var total = await _payRepo.SumPaymentsByOrderIdsAndMethodsAsync(orderIds, visaMethodIds, ct);

            total = Math.Round(total, 2, MidpointRounding.AwayFromZero);

            return new(true,
                "VISA total calculated successfully.",
                request.BranchId,
                request.ShiftId,
                shift.StartTime,
                shift.EndTime ?? DateTime.UtcNow,
                total);
        }

        public async Task<EndDayResponse> EndDayAsync(EndDayRequest request, CancellationToken ct)
        {
            if (request.BranchId <= 0)
                return Fail("BranchId is required.");

            if (request.ShiftId <= 0)
                return Fail("ShiftId is required.");

            if (string.IsNullOrWhiteSpace(request.DeviceId))
                return Fail("DeviceId is required.");

            if (request.ClosingCash < 0)
                return Fail("ClosingCash must be >= 0.");

            var branch = await _menuRepo.GetBranchAsync(request.BranchId, ct);
            if (branch is null)
                return Fail("Branch not found.");

            var shift = await _repo.GetByIdAsync(request.ShiftId, ct);
            if (shift is null)
                return Fail("Shift not found.");

            if (shift.BranchId != request.BranchId)
                return Fail("Shift does not belong to this branch.");

            // 1) Orders in shift
            var orderIds = await _orderRepo.GetOrderIdsByShiftAsync(request.BranchId, request.ShiftId, ct);

            // 2) Methods IDs
            var visaMethodIds = await _payRepo.GetVisaPaymentMethodIdsAsync(branch.ClientId, ct);
            var cashMethodIds = await _payRepo.GetCashPaymentMethodIdsAsync(branch.ClientId, ct);

            // 3) Totals (no dates filter!)
            var visaSales = (orderIds.Count == 0 || visaMethodIds.Count == 0)
                ? 0m
                : await _payRepo.SumPaymentsByOrderIdsAndMethodsAsync(orderIds, visaMethodIds, ct);

            var cashSales = (orderIds.Count == 0 || cashMethodIds.Count == 0)
                ? 0m
                : await _payRepo.SumPaymentsByOrderIdsAndMethodsAsync(orderIds, cashMethodIds, ct);

            // TODO: wire these when you provide tables (returns, cash in/out)
            var returnsAmount = 0m;
            var cashIn = await _cashRepo.SumCashInByShiftAsync(request.BranchId, request.ShiftId, ct);
            var cashOut = await _cashRepo.SumCashOutByShiftAsync(request.BranchId, request.ShiftId, ct);

            // CurrentBalance = OpeningCash + CashSales + CashIn - CashOut - Returns
            var currentBalance = Round(shift.OpeningCash + cashSales + cashIn - cashOut - returnsAmount);

            // FinalBalance = cashier declared ClosingCash
            var finalBalance = Round(request.ClosingCash);

            var lastOrderUtc = await _orderRepo.GetLastOrderCreatedAtByShiftAsync(request.BranchId, request.ShiftId, ct);

            // 4) Close the shift
            shift.EndTime = DateTime.UtcNow;
            shift.ClosingCash = finalBalance;

            // Begin the unit of work. IUnitOfWork.BeginAsync returns Task (no transaction object to assign).
            await _uow.BeginAsync(ct);

            try
            {
                await _repo.UpdateAsync(shift, ct);
                await _uow.SaveChangesAsync(ct);
                await _uow.CommitAsync(ct);
            }
            catch
            {
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

            return new EndDayResponse(
                Success: true,
                Message: "End day calculated successfully.",
                BranchId: request.BranchId,
                ShiftId: request.ShiftId,
                DeviceId: request.DeviceId,
                CurrentDateUtc: DateTime.UtcNow,

                CurrentBalance: currentBalance,
                FinalBalance: finalBalance,

                DayAccounts: new DayAccountsDto(
                    CashSales: Round(cashSales),
                    VisaSales: Round(visaSales),
                    Returns: Round(returnsAmount),
                    CashIn: Round(cashIn),
                    CashOut: Round(cashOut)
                ),

                Totals: new DayTotalsDto(
                    ShiftStartCash: Round(shift.OpeningCash),
                    ShiftEndCash: Round(finalBalance),
                    LastOperationUtc: lastOrderUtc
                )
            );

            EndDayResponse Fail(string msg) =>
                new(false, msg, request.BranchId, request.ShiftId, request.DeviceId, default, 0, 0,
                    new DayAccountsDto(0, 0, 0, 0, 0), new DayTotalsDto(0, 0, null));
        }

        private static decimal Round(decimal v) => Math.Round(v, 2, MidpointRounding.AwayFromZero);
    }
}
