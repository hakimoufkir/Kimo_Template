using System.Linq.Expressions;
using Kimo.ZLApp.Infrastructure.Common.Filtering.Filters.FilterHandlers;
using Shouldly;

namespace Kimo.ZLApp.Infrastructure.UnitTests.Common.Filtering.Filters.FilterHandlers;

public class EqualsFilterHandlerTests
{
    private readonly EqualsFilterHandler _handler = new();

    [Fact]
    public void BuildPredicate_ShouldReturnEqualExpression_WhenValidValue()
    {
        // Arrange
        var parameter = Expression.Parameter(typeof(TestEntity), "x");
        var property = Expression.Property(parameter, nameof(TestEntity.Name));
        var values = new List<string> { "TestValue" };

        // Act
        var predicate = _handler.BuildPredicate(property, values);

        // Assert
        predicate.ShouldNotBeNull();
        var lambda = Expression.Lambda<Func<TestEntity, bool>>(predicate, parameter).Compile();

        var entityMatching = new TestEntity { Name = "TestValue" };
        var entityNonMatching = new TestEntity { Name = "OtherValue" };

        lambda(entityMatching).ShouldBeTrue();
        lambda(entityNonMatching).ShouldBeFalse();
    }

    [Fact]
    public void BuildPredicate_ShouldReturnFalseConstant_WhenInvalidValue()
    {
        // Arrange
        var parameter = Expression.Parameter(typeof(TestEntity), "x");
        var property = Expression.Property(parameter, nameof(TestEntity.Id));
        var values = new List<string> { "not-an-int" };

        // Act
        var predicate = _handler.BuildPredicate(property, values);

        // Assert
        predicate.ShouldNotBeNull();
        predicate.ShouldBeOfType<ConstantExpression>();
        ((ConstantExpression)predicate).Value.ShouldBe(false);
    }

    [Fact]
    public void BuildPredicate_ShouldReturnFalseConstant_WhenEmptyList()
    {
        // Arrange
        var parameter = Expression.Parameter(typeof(TestEntity), "x");
        var property = Expression.Property(parameter, nameof(TestEntity.Name));
        var values = new List<string>();

        // Act
        var predicate = _handler.BuildPredicate(property, values);

        // Assert
        predicate.ShouldNotBeNull();
        predicate.ShouldBeOfType<ConstantExpression>();
        ((ConstantExpression)predicate).Value.ShouldBe(false);
    }

    [Fact]
    public void BuildPredicate_ShouldReturnFalseConstant_WhenValueIsNullOrWhitespace()
    {
        // Arrange
        var parameter = Expression.Parameter(typeof(TestEntity), "x");
        var property = Expression.Property(parameter, nameof(TestEntity.Name));

        var valuesNull = new List<string> { null! };
        var valuesWhitespace = new List<string> { "  " };

        // Act
        var predicateNull = _handler.BuildPredicate(property, valuesNull);
        var predicateWhitespace = _handler.BuildPredicate(property, valuesWhitespace);

        // Assert
        predicateNull.ShouldBeOfType<ConstantExpression>();
        ((ConstantExpression)predicateNull).Value.ShouldBe(false);

        predicateWhitespace.ShouldBeOfType<ConstantExpression>();
        ((ConstantExpression)predicateWhitespace).Value.ShouldBe(false);
    }
}
