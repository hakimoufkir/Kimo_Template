using System.Linq.Expressions;
using Kimo.ZLApp.Infrastructure.Common.Filtering.Filters;
using Kimo.ZLApp.Infrastructure.Common.Filtering.Sorting;

namespace Kimo.ZLApp.Infrastructure.Common.Filtering;

public class QueryConfiguration<T> where T : class
{
    private readonly List<IFilterConfig<T>> _filterConfigs = [];
    private readonly List<SortConfig<T>> _sortConfigs = [];

    public QueryConfiguration<T> AddFilter<TProperty>(
        string fieldName,
        Expression<Func<T, TProperty>> propertyExpression,
        FilterType type = FilterType.Equals)
    {
        _filterConfigs.Add(new FilterConfig<T, TProperty>
        {
            FieldName = fieldName,
            PropertyExpression = propertyExpression,
            Type = type
        });

        return this;
    }

    public QueryConfiguration<T> AddSort(
        string fieldName,
        Expression<Func<T, object>> propertyExpression,
        Expression<Func<T, object>>? thenBy = null)
    {
        _sortConfigs.Add(new SortConfig<T>
        {
            FieldName = fieldName,
            PropertyExpression = propertyExpression,
            ThenByExpression = thenBy
        });

        return this;
    }

    public List<IFilterConfig<T>> GetFilterConfigs()
    {
        return _filterConfigs;
    }

    public List<SortConfig<T>> GetSortConfigs()
    {
        return _sortConfigs;
    }
}
