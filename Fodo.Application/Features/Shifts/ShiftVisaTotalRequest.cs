using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Application.Features.Shifts
{
    public sealed record ShiftVisaTotalRequest(
    int BranchId,
    int ShiftId,
    string DeviceId
);
    public sealed record ShiftVisaTotalResponse(
        bool Success,
        string Message,
        int BranchId,
        int ShiftId,
        DateTime ShiftStartUtc,
        DateTime ShiftEndUtc,
        decimal VisaTotalAmount
    );
}
