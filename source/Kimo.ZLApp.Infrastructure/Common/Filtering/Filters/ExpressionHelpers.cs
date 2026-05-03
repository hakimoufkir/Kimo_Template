using System.Linq.Expressions;

namespace Kimo.ZLApp.Infrastructure.Common.Filtering.Filters;

public static class ExpressionHelpers
{
    public static Expression ConvertTo(Expression expression, Type targetType)
    {
        return expression.Type == targetType ? expression : Expression.Convert(expression, targetType);
    }
}