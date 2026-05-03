using Microsoft.EntityFrameworkCore;
using Kimo.ZLApp.Application.Common.Filtering.Requests;
using Kimo.ZLApp.Infrastructure.Common.Filtering.Pagination;
using Shouldly;

namespace Kimo.ZLApp.Infrastructure.UnitTests.Common.Filtering.Pagination;

public class PaginationExtensionsTests
{
    private DbContextOptions<TestDbContext> CreateInMemoryOptions(string dbName)
    {
        return new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;
    }

    [Fact]
    public async Task ToPaginatedAsync_ShouldReturnPagedData_Correctly()
    {
        // Arrange
        var options = CreateInMemoryOptions("PaginationTestDb1");

        await using var context = new TestDbContext(options);
        context.TestEntities.AddRange(
            new TestEntity { Id = 1, Name = "A" },
            new TestEntity { Id = 2, Name = "B" },
            new TestEntity { Id = 3, Name = "C" }
        );
        await context.SaveChangesAsync();

        var pagination = new PaginationOptions { Skip = 1, Take = 2 };

        // Act
        var result = await context.TestEntities.AsQueryable()
            .ToPaginatedAsync(pagination);

        // Assert
        result.ShouldNotBeNull();
        result.Data.Count.ShouldBe(2);
        result.TotalRecords.ShouldBe(3);
        result.Page.ShouldBe(1);
        result.PageSize.ShouldBe(2);

        result.Data.ElementAt(0).Id.ShouldBe(2);
        result.Data.ElementAt(1).Id.ShouldBe(3);
    }

    [Fact]
    public async Task ToPaginatedAsync_ShouldReturnEmptyList_WhenSkipExceedsTotal()
    {
        // Arrange
        var options = CreateInMemoryOptions("PaginationTestDb2");

        await using var context = new TestDbContext(options);
        context.TestEntities.AddRange(
            new TestEntity { Id = 1, Name = "A" },
            new TestEntity { Id = 2, Name = "B" }
        );
        await context.SaveChangesAsync();

        var pagination = new PaginationOptions { Skip = 5, Take = 2 };

        // Act
        var result = await context.TestEntities.AsQueryable()
            .ToPaginatedAsync(pagination);

        // Assert
        result.Data.ShouldBeEmpty();
        result.TotalRecords.ShouldBe(2);
    }

    [Fact]
    public void ToPaginatedAsync_ShouldThrow_WhenSkipIsNegative()
    {
        // Arrange
        var pagination = new PaginationOptions { Skip = -1, Take = 2 };

        var queryable = new List<TestEntity>().AsQueryable();

        // Act & Assert
        Should.Throw<ArgumentOutOfRangeException>(async () =>
            await queryable.ToPaginatedAsync(pagination));
    }

    [Fact]
    public void ToPaginatedAsync_ShouldThrow_WhenTakeIsZeroOrNegative()
    {
        // Arrange
        var paginationZero = new PaginationOptions { Skip = 0, Take = 0 };
        var paginationNegative = new PaginationOptions { Skip = 0, Take = -1 };
        var queryable = new List<TestEntity>().AsQueryable();

        // Act & Assert
        Should.Throw<ArgumentOutOfRangeException>(async () =>
            await queryable.ToPaginatedAsync(paginationZero));

        Should.Throw<ArgumentOutOfRangeException>(async () =>
            await queryable.ToPaginatedAsync(paginationNegative));
    }
}
