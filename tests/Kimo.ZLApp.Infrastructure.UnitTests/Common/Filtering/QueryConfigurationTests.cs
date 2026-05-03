using Kimo.ZLApp.Infrastructure.Common.Filtering;
using Kimo.ZLApp.Infrastructure.Common.Filtering.Filters;
using Shouldly;

namespace Kimo.ZLApp.Infrastructure.UnitTests.Common.Filtering;

public class QueryConfigurationTests
{
    [Fact]
    public void AddFilter_ShouldAddFilterConfig()
    {
        // Arrange
        var sut = new QueryConfiguration<TestEntity>();

        // Act
        sut.AddFilter("Id", x => x.Id);
        var filterConfigs = sut.GetFilterConfigs();

        // Assert
        filterConfigs.Count.ShouldBe(1);
        var filter = filterConfigs[0];
        filter.ShouldBeOfType<FilterConfig<TestEntity, int>>();
        filter.FieldName.ShouldBe("Id");
    }

    [Fact]
    public void AddFilter_ShouldSetFilterType_WhenSpecified()
    {
        // Arrange
        var sut = new QueryConfiguration<TestEntity>();

        // Act
        sut.AddFilter("Name", x => x.Name, FilterType.Contains);
        var filterConfigs = sut.GetFilterConfigs();

        // Assert
        var filter = filterConfigs[0] as FilterConfig<TestEntity, string>;
        filter?.Type.ShouldBe(FilterType.Contains);
    }

    [Fact]
    public void AddSort_ShouldAddSortConfig()
    {
        // Arrange
        var sut = new QueryConfiguration<TestEntity>();

        // Act
        sut.AddSort("Id", x => x.Id);
        var sortConfigs = sut.GetSortConfigs();

        // Assert
        sortConfigs.Count.ShouldBe(1);
        var sort = sortConfigs[0];
        sort.FieldName.ShouldBe("Id");
        sort.PropertyExpression.ShouldNotBeNull();
        sort.ThenByExpression.ShouldBeNull();
    }

    [Fact]
    public void AddSort_ShouldAddThenBy_WhenSpecified()
    {
        // Arrange
        var sut = new QueryConfiguration<TestEntity>();

        // Act
        sut.AddSort("Name", x => x.Name, x => x.Id);
        var sortConfigs = sut.GetSortConfigs();

        // Assert
        sortConfigs.Count.ShouldBe(1);
        var sort = sortConfigs[0];
        sort.ThenByExpression.ShouldNotBeNull();
    }

    [Fact]
    public void Chaining_AddFilterAndAddSort_ShouldMaintainBothConfigurations()
    {
        // Arrange
        var sut = new QueryConfiguration<TestEntity>();

        // Act
        sut.AddFilter("Id", x => x.Id)
            .AddSort("Name", x => x.Name);

        // Assert
        sut.GetFilterConfigs().Count.ShouldBe(1);
        sut.GetSortConfigs().Count.ShouldBe(1);
    }
}
