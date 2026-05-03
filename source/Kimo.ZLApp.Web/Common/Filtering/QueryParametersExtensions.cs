using Kimo.ZLApp.Application.Common.Filtering.Requests;

namespace Kimo.ZLApp.Web.Common.Filtering;

internal static class QueryParametersExtensions
{
    public static PaginationOptions GetPaginationOptions(this QueryParameters queryParameters)
    {
        return new PaginationOptions { Skip = queryParameters.Skip, Take = queryParameters.Take };
    }

    public static SortingOptions GetSortingOptions(this QueryParameters queryParameters)
    {
        return new SortingOptions
        {
            Field = queryParameters.Sort,
            Direction = queryParameters.Order == "asc" ? SortingDirection.Ascending : SortingDirection.Descending
        };
    }

    public static FilterOptions? GetFilterOptions(this QueryParameters queryParameters)
    {
        return queryParameters.Filters.ToFilterOptions();
    }
}