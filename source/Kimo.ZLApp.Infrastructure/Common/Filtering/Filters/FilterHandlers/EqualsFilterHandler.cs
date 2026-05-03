using System.Linq.Expressions;

namespace Kimo.ZLApp.Infrastructure.Common.Filtering.Filters.FilterHandlers;

public class EqualsFilterHandler : IFilterHandler
{
    public Expression? BuildPredicate(Expression property, List<string> values)
    {
        if (values.Count == 0)
        {
            return Expression.Constant(false);
        }

        var propertyType = property.Type;
        var convertedValue = ValueConverter.ConvertValue(values[0], propertyType);

        if (convertedValue is null)
        {
            return Expression.Constant(false);
        }

        var constant = Expression.Constant(convertedValue, propertyType);
        return Expression.Equal(property, constant);
    }
}