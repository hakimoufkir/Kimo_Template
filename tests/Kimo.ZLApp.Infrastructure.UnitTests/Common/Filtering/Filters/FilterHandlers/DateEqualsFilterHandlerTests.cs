using System.Linq.Expressions;
using Kimo.ZLApp.Infrastructure.Common.Filtering.Filters.FilterHandlers;
using Shouldly;

namespace Kimo.ZLApp.Infrastructure.UnitTests.Common.Filtering.Filters.FilterHandlers;

public class DateEqualsFilterHandlerTests
{
    private readonly DateEqualsFilterHandler _handler = new();

    [Fact]
    public void BuildPredicate_ShouldReturnContainsExpression_WhenValidDates()
    {
        // Arrange
        var property = Expression.Parameter(typeof(TestEntity), "x");
        var dateProperty = Expression.Property(property, nameof(TestEntity.Date));

        var values = new List<string> { "2026-01-05", "2026-01-06" };

        // Act
        var predicate = _handler.BuildPredicate(dateProperty, values);

        // Assert
        predicate.ShouldNotBeNull();
        var lambda = Expression.Lambda<Func<TestEntity, bool>>(predicate!, property).Compile();

        var entity1 = new TestEntity { Date = new DateOnly(2026, 1, 5) };
        var entity2 = new TestEntity { Date = new DateOnly(2026, 1, 6) };
        var entity3 = new TestEntity { Date = new DateOnly(2026, 1, 7) };

        lambda(entity1).ShouldBeTrue();
        lambda(entity2).ShouldBeTrue();
        lambda(entity3).ShouldBeFalse();
    }

    [Fact]
    public void BuildPredicate_ShouldReturnFalseConstant_WhenNoValidDates()
    {
        // Arrange
        var property = Expression.Parameter(typeof(TestEntity), "x");
        var dateProperty = Expression.Property(property, nameof(TestEntity.Date));

        var values = new List<string> { "invalid-date", "another-bad-date" };

        // Act
        var predicate = _handler.BuildPredicate(dateProperty, values);

        // Assert
        predicate.ShouldNotBeNull();
        predicate.ShouldBeOfType<ConstantExpression>();
        ((ConstantExpression)predicate!).Value.ShouldBe(false);
    }

    [Fact]
    public void BuildPredicate_ShouldReturnFalseConstant_WhenEmptyList()
    {
        // Arrange
        var property = Expression.Parameter(typeof(TestEntity), "x");
        var dateProperty = Expression.Property(property, nameof(TestEntity.Date));

        var values = new List<string>();

        // Act
        var predicate = _handler.BuildPredicate(dateProperty, values);

        // Assert
        predicate.ShouldNotBeNull();
        predicate.ShouldBeOfType<ConstantExpression>();
        ((ConstantExpression)predicate!).Value.ShouldBe(false);
    }

    [Fact]
    public void BuildPredicate_ShouldHandleMixedValidAndInvalidDates()
    {
        // Arrange
        var property = Expression.Parameter(typeof(TestEntity), "x");
        var dateProperty = Expression.Property(property, nameof(TestEntity.Date));

        var values = new List<string> { "invalid", "2026-01-06" };

        // Act
        var predicate = _handler.BuildPredicate(dateProperty, values);

        // Assert
        predicate.ShouldNotBeNull();
        var lambda = Expression.Lambda<Func<TestEntity, bool>>(predicate!, property).Compile();

        var entityValid = new TestEntity { Date = new DateOnly(2026, 1, 6) };
        var entityInvalid = new TestEntity { Date = new DateOnly(2026, 1, 5) };

        lambda(entityValid).ShouldBeTrue();
        lambda(entityInvalid).ShouldBeFalse();
    }

    private class TestEntity
    {
        public DateOnly Date { get; set; }
    }
}
