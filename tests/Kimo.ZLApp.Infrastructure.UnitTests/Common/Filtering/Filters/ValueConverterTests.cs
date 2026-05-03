using Kimo.ZLApp.Infrastructure.Common.Filtering.Filters;
using Shouldly;

namespace Kimo.ZLApp.Infrastructure.UnitTests.Common.Filtering.Filters;

public class ValueConverterTests
{
    [Theory]
    [InlineData("123", typeof(int), 123)]
    [InlineData("true", typeof(bool), true)]
    [InlineData("2025-12-12", typeof(DateTime), "2025-12-12")]
    public void ConvertValue_ShouldConvertToTargetType(string input, Type targetType, object expected)
    {
        // Act
        var result = ValueConverter.ConvertValue(input, targetType);

        // Assert
        if (targetType == typeof(DateTime))
        {
            ((DateTime)result!).ShouldBe(DateTime.Parse((string)expected));
        }
        else
        {
            result.ShouldBe(expected);
        }
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void ConvertValue_ShouldReturnNull_ForNullOrWhitespaceInput(string input)
    {
        // Act
        var result = ValueConverter.ConvertValue(input, typeof(int));

        // Assert
        result.ShouldBeNull();
    }

    [Fact]
    public void ConvertValue_ShouldHandleNullableTypes_ForValidInput()
    {
        // Act
        var result = ValueConverter.ConvertValue("123", typeof(int?));

        // Assert
        result.ShouldBe(123);
    }

    [Fact]
    public void ConvertValue_ShouldHandleNullableTypes_ForEmptyInput()
    {
        // Act
        var result = ValueConverter.ConvertValue("", typeof(int?));

        // Assert
        result.ShouldBeNull();
    }

    [Fact]
    public void ConvertValue_ShouldReturnNull_WhenConversionNotSupported()
    {
        // Arrange
        var unsupportedType = typeof(ValueConverterTests);

        // Act
        var result = ValueConverter.ConvertValue("anything", unsupportedType);

        // Assert
        result.ShouldBeNull();
    }
}
