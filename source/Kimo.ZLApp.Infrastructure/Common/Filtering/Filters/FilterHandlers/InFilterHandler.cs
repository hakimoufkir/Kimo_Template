using System.Collections;
using System.Linq.Expressions;
using System.Reflection;

namespace Kimo.ZLApp.Infrastructure.Common.Filtering.Filters.FilterHandlers;

public class InFilterHandler : IFilterHandler
{
    private static readonly MethodInfo s_enumerableContainsMethod =
        typeof(Enumerable)
            .GetMethods(BindingFlags.Static | BindingFlags.Public)
            .First(m => m.Name == nameof(Enumerable.Contains) && m.GetParameters().Length == 2);

    public Expression? BuildPredicate(Expression property, List<string> values)
    {
        if (values.Count == 0)
        {
            return Expression.Constant(false);
        }

        var propertyType = Nullable.GetUnderlyingType(property.Type) ?? property.Type;

        var convertedValues = values
            .Select(v => ValueConverter.ConvertValue(v, propertyType))
            .Where(v => v is not null)
            .ToList();

        if (convertedValues.Count == 0)
        {
            return Expression.Constant(false);
        }

        var listType = typeof(List<>).MakeGenericType(propertyType);
        var typedList = (IList)Activator.CreateInstance(listType)!;

        foreach (var v in convertedValues)
        {
            typedList.Add(v);
        }

        var constant = Expression.Constant(typedList, listType);

        var containsMethod = s_enumerableContainsMethod.MakeGenericMethod(propertyType);

        var convertedProperty = property;
        if (Nullable.GetUnderlyingType(property.Type) is not null)
        {
            convertedProperty = Expression.Condition(
                Expression.Property(property, "HasValue"),
                Expression.Property(property, "Value"),
                Expression.Default(propertyType));
        }

        return Expression.Call(containsMethod, constant, convertedProperty);
    }
}