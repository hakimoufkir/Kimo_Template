using Kimo.ZLApp.Infrastructure.Common.Filtering.Filters.FilterHandlers;

namespace Kimo.ZLApp.Infrastructure.Common.Filtering.Filters;

public static class FilterHandlerFactory
{
    private static readonly Dictionary<FilterType, IFilterHandler> s_handlers = new()
    {
        { FilterType.Equals, new EqualsFilterHandler() },
        { FilterType.In, new InFilterHandler() },
        { FilterType.Contains, new ContainsFilterHandler() },
        { FilterType.DateEquals, new DateEqualsFilterHandler() }
    };

    public static IFilterHandler GetHandler(FilterType filterType)
    {
        return !s_handlers.TryGetValue(filterType, out var handler)
            ? throw new NotSupportedException($"Filter type '{filterType}' is not supported.")
            : handler;
    }
}
