using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Contracts.DTOS
{
    public sealed record CashOutVoucherDto(
    long CashOutVoucherId,
    int BranchId,
    int ShiftId,
    Guid? UserId,
    decimal Amount,
    string? Reason,
    DateTime CreatedAtUtc,
    string DeviceId,
    Guid RowGuid,
    bool IsSynced,
    DateTime? SyncedAtUtc
);
}
