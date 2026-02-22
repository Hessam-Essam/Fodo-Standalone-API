using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Application.Features.Shifts.EndDay
{
    public sealed record EndDayResponse(
    bool Success,
    string Message,

    int BranchId,
    int ShiftId,
    string DeviceId,

    DateTime CurrentDateUtc,

    decimal CurrentBalance,  // الرصيد الحالي
    decimal FinalBalance,    // الرصيد النهائي

    DayAccountsDto DayAccounts, // حسابات اليوم
    DayTotalsDto Totals          // الإجمالي
);

    public sealed record DayAccountsDto(
        decimal CashSales,   // المبيعات النقدية
        decimal VisaSales,   // فيزا
        decimal Returns,     // المرتجع
        decimal CashIn,      // سند قبض
        decimal CashOut      // سند صرف
    );

    public sealed record DayTotalsDto(
        decimal ShiftStartCash,  // بداية استلام الوردية (OpeningCash)
        decimal ShiftEndCash,    // نهاية استلام الوردية (ClosingCash)
        DateTime? LastOperationUtc // وقت آخر عملية
    );
}
