using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Contracts.DTOS
{
    public record BranchesDto(
    int BranchId,
    string Code,
    string NameEn,
    string NameAr,
    string Address,
    string Phone,
    bool IsActive
);
}
