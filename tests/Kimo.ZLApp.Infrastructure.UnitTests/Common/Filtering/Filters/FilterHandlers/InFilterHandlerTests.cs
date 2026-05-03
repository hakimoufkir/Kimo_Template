using System.Linq.Expressions;
using Kimo.ZLApp.Infrastructure.Common.Filtering.Filters.FilterHandlers;
using Shouldly;

namespace Kimo.ZLApp.Infrastructure.UnitTests.Common.Filtering.Filters.FilterHandlers;

public class InFilterHandlerTests
{
    private readonly InFilterHandler _handler = new();

    [Fact]
    public void BuildPredicate_ShouldReturnContainsExpression_WhenAllValidValues()
    {
        // Arrange
        var parameter = Expression.Parameter(typeof(TestEntity), "x");
        var property = Expression.Property(parameter, nameof(TestEntity.IntProperty));
        var values = new List<string> { "1", "2", "3" };

        // Act
        var predicate = _handler.BuildPredicate(property, values);

        // Assert
        predicate.ShouldNotBeNull();
        var lambda = Expression.Lambda<Func<TestEntity, bool>>(predicate, parameter).Compile();

        var matching = new TestEntity { IntProperty = 2 };
        var nonMatching = new TestEntity { IntProperty = 5 };

        lambda(matching).ShouldBeTrue();
        lambda(nonMatching).ShouldBeFalse();
    }

    [Fact]
    public void BuildPredicate_ShouldIgnoreInvalidValues_AndEvaluateRemaining()
    {
        // Arrange
        var parameter = Expression.Parameter(typeof(TestEntity), "x");
        var property = Expression.Property(parameter, nameof(TestEntity.IntProperty));
        var values = new List<string> { "invalid", "2" };

        // Act
        var predicate = _handler.BuildPredicate(property, values);

        // Assert
        predicate.ShouldNotBeNull();
        var lambda = Expression.Lambda<Func<TestEntity, bool>>(predicate, parameter).Compile();

        var matching = new TestEntity { IntProperty = 2 };
        var nonMatching = new TestEntity { IntProperty = 1 };

        lambda(matching).ShouldBeTrue();
        lambda(nonMatching).ShouldBeFalse();
    }

    [Fact]
    public void BuildPredicate_ShouldReturnFalseConstant_WhenAllInvalid()
    {
        // Arrange
        var parameter = Expression.Parameter(typeof(TestEntity), "x");
        var property = Expression.Property(parameter, nameof(TestEntity.IntProperty));
        var values = new List<string> { "invalid", "also-bad" };

        // Act
        var predicate = _handler.BuildPredicate(property, values);

        // Assert
        predicate.ShouldBeOfType<ConstantExpression>();
        ((ConstantExpression)predicate).Value.ShouldBe(false);
    }

    [Fact]
    public void BuildPredicate_ShouldReturnFalseConstant_WhenEmptyList()
    {
        // Arrange
        var parameter = Expression.Parameter(typeof(TestEntity), "x");
        var property = Expression.Property(parameter, nameof(TestEntity.IntProperty));
        var values = new List<string>();

        // Act
        var predicate = _handler.BuildPredicate(property, values);

        // Assert
        predicate.ShouldBeOfType<ConstantExpression>();
        ((ConstantExpression)predicate).Value.ShouldBe(false);
    }

    [Fact]
    public void BuildPredicate_ShouldWorkWithNullableProperty()
    {
        // Arrange
        var parameter = Expression.Parameter(typeof(TestEntity), "x");
        var property = Expression.Property(parameter, nameof(TestEntity.NullableInt));
        var values = new List<string> { "1", "2" };

        // Act
        var predicate = _handler.BuildPredicate(property, values);

        // Assert
        predicate.ShouldNotBeNull();
        var lambda = Expression.Lambda<Func<TestEntity, bool>>(predicate, parameter).Compile();

        var matching = new TestEntity { NullableInt = 2 };
        var nonMatching = new TestEntity { NullableInt = 5 };
        var nullValue = new TestEntity { NullableInt = null };

        lambda(matching).ShouldBeTrue();
        lambda(nonMatching).ShouldBeFalse();
        lambda(nullValue).ShouldBeFalse();
    }

    private class TestEntity
    {
        public int IntProperty { get; set; }
        public int? NullableInt { get; set; }
    }
}
