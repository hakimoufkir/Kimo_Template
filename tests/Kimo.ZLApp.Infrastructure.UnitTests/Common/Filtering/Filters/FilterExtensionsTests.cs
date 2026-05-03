using Kimo.ZLApp.Application.Common.Filtering.Requests;
using Kimo.ZLApp.Infrastructure.Common.Filtering;
using Kimo.ZLApp.Infrastructure.Common.Filtering.Filters;
using Shouldly;

namespace Kimo.ZLApp.Infrastructure.UnitTests.Common.Filtering.Filters;

public class FilterExtensionsTests
{
    [Fact]
    public void ApplyFilters_ShouldReturnOriginalQuery_WhenNoFiltersProvided()
    {
        // Arrange
        var data = new List<TestEntity> { new() { Id = 1, Name = "A" }, new() { Id = 2, Name = "B" } }.AsQueryable();

        var config = new QueryConfiguration<TestEntity>();

        // Act
        var result = data.ApplyFilters(null, config);

        // Assert
        result.ShouldBe(data);
    }

    [Fact]
    public void ApplyFilters_ShouldReturnOriginalQuery_WhenConfigurationEmpty()
    {
        // Arrange
        var data = new List<TestEntity> { new() { Id = 1, Name = "A" }, new() { Id = 2, Name = "B" } }.AsQueryable();

        var filterOptions = new FilterOptions
        {
            Filters = new List<FilterOption> { new() { Field = "Id", Value = "1" } }
        };

        var config = new QueryConfiguration<TestEntity>();

        // Act
        var result = data.ApplyFilters(filterOptions, config);

        // Assert
        result.ShouldBe(data);
    }

    [Fact]
    public void ApplyFilters_ShouldApplyEqualsFilter_OnMatchingField()
    {
        // Arrange
        var data = new List<TestEntity> { new() { Id = 1, Name = "A" }, new() { Id = 2, Name = "B" } }.AsQueryable();

        var config = new QueryConfiguration<TestEntity>()
            .AddFilter("Id", x => x.Id);

        var filterOptions = new FilterOptions
        {
            Filters = new List<FilterOption> { new() { Field = "Id", Value = "1" } }
        };

        // Act
        var result = data.ApplyFilters(filterOptions, config).ToList();

        // Assert
        result.Count.ShouldBe(1);
        result[0].Id.ShouldBe(1);
    }

    [Fact]
    public void ApplyFilters_ShouldIgnoreNonMatchingField()
    {
        // Arrange
        var data = new List<TestEntity> { new() { Id = 1, Name = "A" }, new() { Id = 2, Name = "B" } }.AsQueryable();

        var config = new QueryConfiguration<TestEntity>()
            .AddFilter("Id", x => x.Id);

        var filterOptions = new FilterOptions
        {
            Filters = new List<FilterOption> { new() { Field = "NonExistingField", Value = "1" } }
        };

        // Act
        var result = data.ApplyFilters(filterOptions, config).ToList();

        // Assert
        result.Count.ShouldBe(2);
    }

    [Fact]
    public void ApplyFilters_ShouldSplitCommaSeparatedValues_AndTrim()
    {
        // Arrange
        var data = new List<TestEntity>
        {
            new() { Id = 1, Name = "A" }, new() { Id = 2, Name = "B" }, new() { Id = 3, Name = "C" }
        }.AsQueryable();

        var config = new QueryConfiguration<TestEntity>()
            .AddFilter("Id", x => x.Id, FilterType.In);

        var filterOptions = new FilterOptions
        {
            Filters = new List<FilterOption> { new() { Field = "Id", Value = " 1 , 3 " } }
        };

        // Act
        var result = data.ApplyFilters(filterOptions, config).ToList();

        // Assert
        result.Count.ShouldBe(2);
        result.Select(x => x.Id).ShouldContain(1);
        result.Select(x => x.Id).ShouldContain(3);
    }

    [Fact]
    public void ApplyFilters_ShouldSkipEmptyOrWhitespaceValues()
    {
        // Arrange
        var data = new List<TestEntity> { new() { Id = 1, Name = "A" }, new() { Id = 2, Name = "B" } }.AsQueryable();

        var config = new QueryConfiguration<TestEntity>()
            .AddFilter("Id", x => x.Id);

        var filterOptions = new FilterOptions
        {
            Filters = new List<FilterOption> { new() { Field = "Id", Value = " " } }
        };

        // Act
        var result = data.ApplyFilters(filterOptions, config).ToList();

        // Assert
        result.Count.ShouldBe(2);
    }

    [Fact]
    public void ApplyFilters_ShouldSkipFilter_WhenValuesSplitToEmpty()
    {
        // Arrange
        var data = new List<TestEntity> { new() { Id = 1, Name = "A" }, new() { Id = 2, Name = "B" } }.AsQueryable();

        var config = new QueryConfiguration<TestEntity>()
            .AddFilter("Id", x => x.Id, FilterType.In);

        var filterOptions = new FilterOptions
        {
            Filters = new List<FilterOption>
            {
                new() { Field = "Id", Value = " , , " }
            }
        };

        // Act
        var result = data.ApplyFilters(filterOptions, config).ToList();

        // Assert
        result.Count.ShouldBe(2);
        result.Select(x => x.Id).ShouldContain(1);
        result.Select(x => x.Id).ShouldContain(2);
    }
}
