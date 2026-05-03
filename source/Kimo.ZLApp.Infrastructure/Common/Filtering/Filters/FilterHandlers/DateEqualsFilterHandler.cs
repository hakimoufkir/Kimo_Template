using System.Linq.Expressions;

namespace Kimo.ZLApp.Infrastructure.Common.Filtering.Filters.FilterHandlers;

public class DateEqualsFilterHandler : IFilterHandler
{
    public Expression? BuildPredicate(Expression property, List<string> values)
    {
        var dates = values
            .Select(v => ValueConverter.ConvertValue(v, typeof(DateOnly)))
            .Where(v => v is not null)
            .Cast<DateOnly>()
            .ToList();

        if (dates.Count == 0)
        {
            return Expression.Constant(false);
        }

        var constant = Expression.Constant(dates);
        var containsMethod = typeof(List<DateOnly>)
            .GetMethod("Contains", [typeof(DateOnly)])!;
        var convertedProperty = ExpressionHelpers.ConvertTo(property, typeof(DateOnly));

        return Expression.Call(constant, containsMethod, convertedProperty);
    }
}