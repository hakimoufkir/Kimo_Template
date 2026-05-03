using Kimo.ZLApp.Application.Common.Filtering.Requests;

namespace Kimo.ZLApp.Web.Common.Filtering;

internal static class FilterOptionsExtensions
{
    internal static FilterOptions? ToFilterOptions(this IDictionary<string, string?>? filters)
    {
        return filters is null
            ? null
            : new FilterOptions
            {
                Filters = filters
                    .Select(kvp => new FilterOption { Field = kvp.Key, Value = kvp.Value })
                    .ToList()
            };
    }
}