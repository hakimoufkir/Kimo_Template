using Kimo.ZLApp.Application.Common.Filtering.Requests;
using Kimo.ZLApp.Domain.WeatherForecasts;
using Kimo.ZLApp.Infrastructure.UnitTests.Common;
using Kimo.ZLApp.Infrastructure.WeatherForecasts;
using Kimo.ZLApp.Tests.Common.Forecasts;
using Kimo.ZLApp.Tests.Common.Locations;
using Shouldly;

namespace Kimo.ZLApp.Infrastructure.UnitTests.WeatherForecasts;

public class ForecastsRepositoryTests
{
    private static async Task<TestDbContext> GenerateMockedContext()
    {
        var context = await TestDbContext.Create();

        var location = TestLocation.Default;

        context.Locations.Add(location);
        context.Forecasts.AddRange(
            TestForecast.Default(location),
            TestForecast.Alternative(location)
        );

        await context.SaveChangesAsync();
        return context;
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnPaginatedForecasts()
    {
        // Arrange
        await using var context = await GenerateMockedContext();

        var repository = new ForecastRepository(context);

        var pagination = new PaginationOptions { Skip = 0, Take = 1 };
        var sorting = new SortingOptions
        {
            Field = nameof(Forecast.Date),
            Direction = SortingDirection.Ascending
        };

        // Act
        var result = await repository.GetAllAsync(pagination, sorting, null);

        // Assert
        result.Data.Count.ShouldBe(1);
        result.TotalRecords.ShouldBe(2);
        result.Data.ElementAt(0).ShouldBeEquivalentTo(TestForecast.Default(TestLocation.Default));
    }

    [Fact]
    public async Task GetByDateForLocationAsync_ShouldReturnCorrectForecast()
    {
        // Arrange
        await using var context = await GenerateMockedContext();

        var repository = new ForecastRepository(context);

        // Act
        var result = await repository.GetByDateForLocationAsync(1, new DateOnly(2026, 1, 6));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeEquivalentTo(TestForecast.Alternative(TestLocation.Default));
    }

    [Fact]
    public async Task RemoveAllBeforeDateAsync_ShouldRemoveMatchingForecasts()
    {
        // Arrange
        await using var context = await GenerateMockedContext();

        var repository = new ForecastRepository(context);

        // Act
        var deleted = await repository.RemoveAllBeforeDateAsync(new DateOnly(2026, 1, 6));

        // Assert
        deleted.ShouldBe(1);
        context.Forecasts.Count().ShouldBe(1);
    }
}
