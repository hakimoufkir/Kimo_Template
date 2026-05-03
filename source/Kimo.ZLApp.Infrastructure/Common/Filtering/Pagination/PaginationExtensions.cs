using Microsoft.EntityFrameworkCore;
using Kimo.ZLApp.Application.Common.Filtering.Requests;
using Kimo.ZLApp.Application.Common.Filtering.Responses;

namespace Kimo.ZLApp.Infrastructure.Common.Filtering.Pagination;

public static class PaginationExtensions
{
    public static async Task<PaginatedData<T>> ToPaginatedAsync<T>(
        this IQueryable<T> query,
        PaginationOptions pagination,
        CancellationToken cancellationToken = default) where T : class
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(pagination.Skip, 0);
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(pagination.Take, 0);

        var totalCount = await query.CountAsync(cancellationToken);

        var s = query.ToQueryString();

        var pagedElements = await query
            .Skip(pagination.Skip)
            .Take(pagination.Take)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return new PaginatedData<T>(
            pagedElements,
            pagination.Skip,
            pagination.Take,
            totalCount);
    }
}
