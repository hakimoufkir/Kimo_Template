using System.ComponentModel;

namespace Kimo.ZLApp.Infrastructure.Common.Filtering.Filters;

public static class ValueConverter
{
    public static object? ConvertValue(string value, Type targetType)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        targetType = Nullable.GetUnderlyingType(targetType) ?? targetType;

        try
        {
            var converter = TypeDescriptor.GetConverter(targetType);
            return converter.ConvertFromInvariantString(value);
        }
        catch
        {
            return null;
        }
    }
}
