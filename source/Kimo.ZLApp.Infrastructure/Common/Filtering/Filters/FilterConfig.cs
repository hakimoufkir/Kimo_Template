using System.Linq.Expressions;

namespace Kimo.ZLApp.Infrastructure.Common.Filtering.Filters;

public sealed record FilterConfig<T, TProperty> : IFilterConfig<T> where T : class
{
    public required Expression<Func<T, TProperty>> PropertyExpression { get; init; }
    public required string FieldName { get; init; }

    public required FilterType Type { get; init; }

    public IQueryable<T> Apply(IQueryable<T> query, List<string> values)
    {
        if (values.Count == 0)
        {
            return query;
        }

        var parameter = Expression.Parameter(typeof(T), "x");
        var property = Expression.Invoke(PropertyExpression, parameter);

        var handler = FilterHandlerFactory.GetHandler(Type);
        var predicate = handler.BuildPredicate(property, values);

        if (predicate is null)
        {
            return query;
        }

        var lambda = Expression.Lambda<Func<T, bool>>(predicate, parameter);
        return query.Where(lambda);
    }
}
