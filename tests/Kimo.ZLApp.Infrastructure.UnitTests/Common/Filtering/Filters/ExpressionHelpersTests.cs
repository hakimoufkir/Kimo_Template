using System.Linq.Expressions;
using Kimo.ZLApp.Infrastructure.Common.Filtering.Filters;
using Shouldly;

namespace Kimo.ZLApp.Infrastructure.UnitTests.Common.Filtering.Filters;

public class ExpressionHelpersTests
{
    [Fact]
    public void ConvertTo_ShouldReturnSameExpression_WhenTypeMatches()
    {
        // Arrange
        Expression<Func<int>> expr = () => 5;

        // Act
        var result = ExpressionHelpers.ConvertTo(expr.Body, typeof(int));

        // Assert
        result.ShouldBe(expr.Body);
        result.Type.ShouldBe(typeof(int));
    }

    [Fact]
    public void ConvertTo_ShouldReturnConvertedExpression_WhenTypeDiffers()
    {
        // Arrange
        Expression<Func<int>> expr = () => 5;

        // Act
        var result = ExpressionHelpers.ConvertTo(expr.Body, typeof(double));

        // Assert
        result.ShouldNotBe(expr.Body);
        result.Type.ShouldBe(typeof(double));
        result.ShouldBeOfType<UnaryExpression>();
        ((UnaryExpression)result).Operand.ShouldBe(expr.Body);
    }

    [Fact]
    public void ConvertTo_ShouldWorkWithReferenceTypes()
    {
        // Arrange
        Expression<Func<string>> expr = () => "test";

        // Act
        var result = ExpressionHelpers.ConvertTo(expr.Body, typeof(object));

        // Assert
        result.Type.ShouldBe(typeof(object));
        result.ShouldBeOfType<UnaryExpression>();
        ((UnaryExpression)result).Operand.ShouldBe(expr.Body);
    }
}
