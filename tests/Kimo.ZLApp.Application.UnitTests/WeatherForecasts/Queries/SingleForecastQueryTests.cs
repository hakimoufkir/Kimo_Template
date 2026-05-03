using Microsoft.Extensions.Logging;
using Kimo.ZLApp.Application.Common.Results.Error;
using Kimo.ZLApp.Application.WeatherForecasts;
using Kimo.ZLApp.Application.WeatherForecasts.Mappers;
using Kimo.ZLApp.Application.WeatherForecasts.Queries;
using Kimo.ZLApp.Domain.WeatherForecasts;
using Kimo.ZLApp.Tests.Common.Forecasts;
using Kimo.ZLApp.Tests.Common.Locations;

namespace Kimo.ZLApp.Application.UnitTests.WeatherForecasts.Queries;

public class SingleForecastQueryTests
{
    private readonly IForecastRepository _forecastRepository = Substitute.For<IForecastRepository>();

    private readonly ILogger<SingleForecastQueryHandler> _logger =
        Substitute.For<ILogger<SingleForecastQueryHandler>>();

    private SingleForecastQueryHandler CreateHandler()
    {
        return new SingleForecastQueryHandler(_forecastRepository, _logger);
    }

    [Fact]
    public async Task HandleRequest_WhenForecastExists_ReturnsOk()
    {
        // Arrange
        _forecastRepository.FindAsync(1).Returns(TestForecast.Default(TestLocation.Default));

        var handler = CreateHandler();
        var query = new SingleForecastQuery { Id = 1 };

        // Act
        var result = await handler.HandleAsync(query);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.ShouldBe(TestForecast.Default(TestLocation.Default).ToDto());
    }

    [Fact]
    public async Task HandleAsync_WhenForecastNotFound_ReturnsNotFoundError()
    {
        // Arrange
        _forecastRepository.FindAsync(1, Arg.Any<CancellationToken>())
            .Returns((Forecast?)null);

        var handler = CreateHandler();
        var query = new SingleForecastQuery { Id = 1 };

        // Act
        var result = await handler.HandleAsync(query);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBe(Error.NotFound());
    }
}
