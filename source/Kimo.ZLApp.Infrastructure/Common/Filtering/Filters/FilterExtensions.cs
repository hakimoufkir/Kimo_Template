using Kimo.ZLApp.Application.Common.Filtering.Requests;

namespace Kimo.ZLApp.Infrastructure.Common.Filtering.Filters;

public static class FilterExtensions
{
    public static IQueryable<T> ApplyFilters<T>(
        this IQueryable<T> query,
        FilterOptions? filterOptions,
        QueryConfiguration<T> configuration)
        where T : class
    {
        if (filterOptions?.Filters.Count is null or 0)
        {
            return query;
        }

        var filterConfigs = configuration.GetFilterConfigs();
        if (filterConfigs.Count == 0)
        {
            return query;
        }

        foreach (var filter in filterOptions.Filters)
        {
            var config = filterConfigs
                .FirstOrDefault(c => string.Equals(c.FieldName, filter.Field, StringComparison.OrdinalIgnoreCase));

            if (config is null || string.IsNullOrWhiteSpace(filter.Value))
            {
                continue;
            }

            var values = filter.Value.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(v => v.Trim())
                .Where(v => !string.IsNullOrEmpty(v))
                .ToList();

            if (values.Count == 0)
            {
                continue;
            }

            query = config.Apply(query, values);
        }

        return query;
    }
}
