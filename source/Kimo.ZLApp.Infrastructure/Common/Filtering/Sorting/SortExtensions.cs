using Kimo.ZLApp.Application.Common.Filtering.Requests;

namespace Kimo.ZLApp.Infrastructure.Common.Filtering.Sorting;

public static class SortExtensions
{
    public static IOrderedQueryable<T> ApplySort<T>(
        this IQueryable<T> query,
        SortingOptions sorting,
        QueryConfiguration<T> configuration) where T : class
    {
        var sortConfigs = configuration.GetSortConfigs();
        ArgumentOutOfRangeException.ThrowIfZero(sortConfigs.Count);

        var config = sortConfigs.FirstOrDefault(c =>
            string.Equals(c.FieldName, sorting.Field, StringComparison.OrdinalIgnoreCase));
        ArgumentNullException.ThrowIfNull(config);

        var ordered = sorting.Direction is SortingDirection.Ascending
            ? query.OrderBy(config.PropertyExpression)
            : query.OrderByDescending(config.PropertyExpression);

        if (config.ThenByExpression is not null)
        {
            ordered = ordered.ThenBy(config.ThenByExpression);
        }

        return ordered;
    }
}
