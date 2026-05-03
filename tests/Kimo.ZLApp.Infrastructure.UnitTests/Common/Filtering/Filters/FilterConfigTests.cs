using Kimo.ZLApp.Infrastructure.Common.Filtering.Filters;
using Shouldly;

namespace Kimo.ZLApp.Infrastructure.UnitTests.Common.Filtering.Filters;

public class FilterConfigTests
{
    [Fact]
    public void Apply_ShouldFilterQuery_WithEqualsFilterHandler()
    {
        // Arrange
        var data = new List<TestEntity> { new() { Id = 1, Name = "A" }, new() { Id = 2, Name = "B" } }.AsQueryable();

        var filterConfig = new FilterConfig<TestEntity, int>
        {
            FieldName = "Id",
            Type = FilterType.Equals,
            PropertyExpression = x => x.Id
        };

        // Act
        var result = filterConfig.Apply(data, ["1"]).ToList();

        // Assert
        result.Count.ShouldBe(1);
        result[0].Id.ShouldBe(1);
    }

    [Fact]
    public void Apply_ShouldReturnAll_WhenNoValuesProvided()
    {
        // Arrange
        var data = new List<TestEntity> { new() { Id = 1, Name = "A" }, new() { Id = 2, Name = "B" } }.AsQueryable();

        var filterConfig = new FilterConfig<TestEntity, int>
        {
            FieldName = "Id",
            Type = FilterType.Equals,
            PropertyExpression = x => x.Id
        };

        // Act
        var result = filterConfig.Apply(data, []).ToList();

        // Assert
        result.Count.ShouldBe(2);
    }
}
