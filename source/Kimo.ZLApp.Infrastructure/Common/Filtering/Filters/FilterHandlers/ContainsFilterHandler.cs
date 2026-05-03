using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Kimo.ZLApp.Infrastructure.Common.Filtering.Filters.FilterHandlers;

public class ContainsFilterHandler : IFilterHandler
{
    private static readonly MethodInfo s_likeMethod =
        typeof(DbFunctionsExtensions)
            .GetMethod(nameof(DbFunctionsExtensions.Like),
                [typeof(DbFunctions), typeof(string), typeof(string)])!;

    public Expression? BuildPredicate(Expression property, List<string> values)
    {
        if (values.Count == 0)
        {
            return null;
        }

        var stringProp =
            property.Type == typeof(string)
                ? property
                : Expression.Call(property, nameof(ToString), Type.EmptyTypes);

        stringProp = Expression.Coalesce(stringProp, Expression.Constant(""));

        var ef = Expression.Constant(EF.Functions);

        Expression? combined = null;
        foreach (var v in values)
        {
            if (string.IsNullOrWhiteSpace(v))
            {
                continue;
            }

            var pattern = Expression.Constant("%" + EscapeLikePattern(v) + "%");

            var like = Expression.Call(s_likeMethod, ef, stringProp, pattern);

            combined = combined is null
                ? like
                : Expression.OrElse(combined, like);
        }

        return combined;
    }

    private static string EscapeLikePattern(string value)
    {
        return value.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]");
    }
}