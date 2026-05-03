using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Kimo.ZLApp.Infrastructure.Common.Filtering.Filters.FilterHandlers;
using Shouldly;

namespace Kimo.ZLApp.Infrastructure.UnitTests.Common.Filtering.Filters.FilterHandlers;

public class ContainsFilterHandlerTests
{
    private readonly ContainsFilterHandler _handler = new();

    private async Task<TestDbContext> CreateDbContextWithEntities(params TestEntity[] entities)
    {
        var context = await TestDbContext.Create();

        context.TestEntities.AddRange(entities);
        await context.SaveChangesAsync();

        return context;
    }

    [Fact]
    public async Task BuildPredicate_ShouldReturnMatchingEntities()
    {
        // Arrange
        var entities = new[]
        {
            new TestEntity { Id = 1, Name = "abc" }, new TestEntity { Id = 2, Name = "xyz" },
            new TestEntity { Id = 3, Name = "abcd" },
        };

        await using var context = await CreateDbContextWithEntities(entities);

        var parameter = Expression.Parameter(typeof(TestEntity), "x");
        var property = Expression.Property(parameter, nameof(TestEntity.Name));
        var values = new List<string> { "abc" };

        var predicateExpr = _handler.BuildPredicate(property, values);
        predicateExpr.ShouldNotBeNull();

        var lambda = Expression.Lambda<Func<TestEntity, bool>>(predicateExpr, parameter);

        // Act
        var result = await context.TestEntities.Where(lambda).ToListAsync();

        // Assert
        result.Select(e => e.Id).ShouldBe([1, 3]);
    }

    [Fact]
    public async Task BuildPredicate_ShouldReturnEmpty_WhenNoMatches()
    {
        // Arrange
        var entities = new[] { new TestEntity { Id = 1, Name = "foo" }, new TestEntity { Id = 2, Name = "bar" }, };

        await using var context = await CreateDbContextWithEntities(entities);

        var parameter = Expression.Parameter(typeof(TestEntity), "x");
        var property = Expression.Property(parameter, nameof(TestEntity.Name));
        var values = new List<string> { "baz" };

        var predicateExpr = _handler.BuildPredicate(property, values);
        predicateExpr.ShouldNotBeNull();

        var lambda = Expression.Lambda<Func<TestEntity, bool>>(predicateExpr, parameter);

        // Act
        var result = await context.Set<TestEntity>().Where(lambda).ToListAsync();

        // Assert
        result.ShouldBeEmpty();
    }

    [Fact]
    public async Task BuildPredicate_ShouldReturnNull_WhenValuesEmpty()
    {
        // Arrange
        await using var context = await CreateDbContextWithEntities();

        var parameter = Expression.Parameter(typeof(TestEntity), "x");
        var property = Expression.Property(parameter, nameof(TestEntity.Name));
        var values = new List<string>();

        // Act
        var predicateExpr = _handler.BuildPredicate(property, values);

        // Assert
        predicateExpr.ShouldBeNull();
    }

    [Fact]
    public async Task BuildPredicate_ShouldHandleMultipleValues()
    {
        // Arrange
        var entities = new[]
        {
            new TestEntity { Id = 1, Name = "apple" }, new TestEntity { Id = 2, Name = "banana" },
            new TestEntity { Id = 3, Name = "grape" },
        };

        await using var context = await CreateDbContextWithEntities(entities);

        var parameter = Expression.Parameter(typeof(TestEntity), "x");
        var property = Expression.Property(parameter, nameof(TestEntity.Name));
        var values = new List<string> { "apple", "grape" };

        var predicateExpr = _handler.BuildPredicate(property, values);
        predicateExpr.ShouldNotBeNull();

        var lambda = Expression.Lambda<Func<TestEntity, bool>>(predicateExpr, parameter);

        // Act
        var result = await context.Set<TestEntity>().Where(lambda).ToListAsync();

        // Assert
        result.Select(e => e.Id).ShouldBe([1, 3]);
    }

    [Fact]
    public async Task BuildPredicate_ShouldSkipEmptyOrWhitespaceValues()
    {
        // Arrange
        var entities = new[]
        {
            new TestEntity { Id = 1, Name = "apple" }, new TestEntity { Id = 2, Name = "banana" },
            new TestEntity { Id = 3, Name = "grape" },
        };

        await using var context = await CreateDbContextWithEntities(entities);

        var parameter = Expression.Parameter(typeof(TestEntity), "x");
        var property = Expression.Property(parameter, nameof(TestEntity.Name));

        var values = new List<string> { "", "  ", "banana" };

        var predicateExpr = _handler.BuildPredicate(property, values);
        predicateExpr.ShouldNotBeNull();

        var lambda = Expression.Lambda<Func<TestEntity, bool>>(predicateExpr, parameter);

        // Act
        var result = await context.Set<TestEntity>().Where(lambda).ToListAsync();

        // Assert
        result.Select(e => e.Id).ShouldBe([2]);
    }
}
