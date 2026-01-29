using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Contracts.DTOS
{
    public record ClientsDto(int ClientId, IReadOnlyList<BranchesDto> Branches);
}
