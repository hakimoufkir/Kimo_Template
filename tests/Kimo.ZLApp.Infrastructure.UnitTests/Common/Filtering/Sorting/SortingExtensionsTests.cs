using Kimo.ZLApp.Application.Common.Filtering.Requests;
using Kimo.ZLApp.Infrastructure.Common.Filtering;
using Kimo.ZLApp.Infrastructure.Common.Filtering.Sorting;
using Shouldly;

namespace Kimo.ZLApp.Infrastructure.UnitTests.Common.Filtering.Sorting;

public class SortExtensionsTests
{
    [Fact]
    public void ApplySort_ShouldSortAscending_ByProperty()
    {
        // Arrange
        var items = new List<TestEntity> { new() { Id = 2, Name = "B" }, new() { Id = 1, Name = "A" } }.AsQueryable();

        var config = new QueryConfiguration<TestEntity>()
            .AddSort("Id", x => x.Id);

        var sorting = new SortingOptions { Field = "Id", Direction = SortingDirection.Ascending };

        // Act
        var result = items.ApplySort(sorting, config).ToList();

        // Assert
        result.Count.ShouldBe(2);
        result[0].Id.ShouldBe(1);
        result[1].Id.ShouldBe(2);
    }

    [Fact]
    public void ApplySort_ShouldSortDescending_ByProperty()
    {
        // Arrange
        var items = new List<TestEntity> { new() { Id = 1, Name = "A" }, new() { Id = 2, Name = "B" } }.AsQueryable();

        var config = new QueryConfiguration<TestEntity>()
            .AddSort("Id", x => x.Id);

        var sorting = new SortingOptions { Field = "Id", Direction = SortingDirection.Descending };

        // Act
        var result = items.ApplySort(sorting, config).ToList();

        // Assert
        result[0].Id.ShouldBe(2);
        result[1].Id.ShouldBe(1);
    }

    [Fact]
    public void ApplySort_ShouldApplyThenBy_WhenConfigured()
    {
        // Arrange
        var items = new List<TestEntity>
        {
            new() { Id = 2, Name = "B" }, new() { Id = 1, Name = "B" }, new() { Id = 1, Name = "A" }
        }.AsQueryable();

        var config = new QueryConfiguration<TestEntity>()
            .AddSort("Name", x => x!.Name, x => x.Id);

        var sorting = new SortingOptions { Field = "Name", Direction = SortingDirection.Ascending };

        // Act
        var result = items.ApplySort(sorting, config).ToList();

        // Assert
        result[0].Name.ShouldBe("A");
        result[1].Name.ShouldBe("B");
        result[1].Id.ShouldBe(1);
        result[2].Id.ShouldBe(2);
    }

    [Fact]
    public void ApplySort_ShouldThrow_WhenNoSortConfigExists()
    {
        // Arrange
        var items = new List<TestEntity>().AsQueryable();
        var config = new QueryConfiguration<TestEntity>();
        var sorting = new SortingOptions { Field = "Id", Direction = SortingDirection.Ascending };

        // Act & Assert
        Should.Throw<ArgumentOutOfRangeException>(() =>
            items.ApplySort(sorting, config));
    }

    [Fact]
    public void ApplySort_ShouldThrow_WhenFieldDoesNotMatchConfig()
    {
        // Arrange
        var items = new List<TestEntity>().AsQueryable();
        var config = new QueryConfiguration<TestEntity>()
            .AddSort("Id", x => x.Id);

        var sorting = new SortingOptions { Field = "NonExistingField", Direction = SortingDirection.Ascending };

        // Act & Assert
        Should.Throw<ArgumentNullException>(() =>
            items.ApplySort(sorting, config));
    }
}
