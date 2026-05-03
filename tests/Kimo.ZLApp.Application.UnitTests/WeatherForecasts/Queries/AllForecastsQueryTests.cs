using Microsoft.Extensions.Logging;
using Kimo.ZLApp.Application.Common.Filtering.Requests;
using Kimo.ZLApp.Application.Common.Filtering.Responses;
using Kimo.ZLApp.Application.WeatherForecasts;
using Kimo.ZLApp.Application.WeatherForecasts.Mappers;
using Kimo.ZLApp.Application.WeatherForecasts.Models;
using Kimo.ZLApp.Application.WeatherForecasts.Queries;
using Kimo.ZLApp.Domain.WeatherForecasts;
using Kimo.ZLApp.Tests.Common.Forecasts;
using Kimo.ZLApp.Tests.Common.Locations;

namespace Kimo.ZLApp.Application.UnitTests.WeatherForecasts.Queries;

public class AllForecastsQueryTests
{
    private readonly IForecastRepository _forecastRepository = Substitute.For<IForecastRepository>();

    private readonly ILogger<AllForecastsQueryHandler> _logger =
        Substitute.For<ILogger<AllForecastsQueryHandler>>();

    private AllForecastsQueryHandler CreateHandler()
    {
        return new AllForecastsQueryHandler(_forecastRepository, _logger);
    }

    [Fact]
    public async Task HandleRequest_ReturnsAll()
    {
        // Arrange
        var paginatedResult = new PaginatedData<Forecast>(
            [TestForecast.Default(TestLocation.Default), TestForecast.Alternative(TestLocation.Default)], 0, 10, 2);

        var paginatedDtoResult = new PaginatedData<ForecastDto>(
        [
            TestForecast.Default(TestLocation.Default).ToDto(), TestForecast.Alternative(TestLocation.Default).ToDto()
        ], 0, 10, 2);

        _forecastRepository.GetAllAsync(Arg.Any<PaginationOptions>(), Arg.Any<SortingOptions>(),
                Arg.Any<FilterOptions?>())
            .Returns(paginatedResult);

        var handler = CreateHandler();
        var query = new AllForecastsQuery
        {
            Pagination = new PaginationOptions { Skip = 0, Take = 10 },
            Sorting = new SortingOptions { Direction = SortingDirection.Ascending, Field = "location" },
            Filter = new FilterOptions { Filters = new List<FilterOption>() }
        };

        // Act
        var result = await handler.HandleAsync(query);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.Data.Count.ShouldBe(2);
        result.Value.ShouldBeEquivalentTo(paginatedDtoResult);
    }
}
