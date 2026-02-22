using System;
using System.Collections.Generic;
using System.Text;

namespace Fodo.Application.Features.Pagination
{
    public sealed record PagedResponse<T>(
     bool Success,
     string Message,
     IReadOnlyList<T> Data,
     int Page,
     int PageSize,
     int TotalCount,
     int TotalPages,
     bool HasNext,
     bool HasPrevious
 );

}
