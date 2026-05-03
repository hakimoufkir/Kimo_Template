using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Time.Testing;
using Kimo.ZLApp.Application.Common;
using Kimo.ZLApp.Application.Common.Results.Error;
using Kimo.ZLApp.Application.Locations;
using Kimo.ZLApp.Application.WeatherForecasts;
using Kimo.ZLApp.Application.WeatherForecasts.Commands;
using Kimo.ZLApp.Application.WeatherForecasts.Mappers;
using Kimo.ZLApp.Domain.Locations;
using Kimo.ZLApp.Domain.WeatherForecasts;
using Kimo.ZLApp.Tests.Common.Locations;

namespace Kimo.ZLApp.Application.UnitTests.WeatherForecasts.Commands;

public class CreateForecastCommandHandlerTests
{
    private static readonly DateTime s_currentDateTime = new(2026, 2, 1, 15, 20, 30, DateTimeKind.Local);
    private readonly IForecastRepository _forecastRepository = Substitute.For<IForecastRepository>();
    private readonly ILocationRepository _locationRepository = Substitute.For<ILocationRepository>();

    private readonly ILogger<CreateForecastCommandHandler> _logger =
        Substitute.For<ILogger<CreateForecastCommandHandler>>();

    private readonly TimeProvider _timeProvider = new FakeTimeProvider(s_currentDateTime);
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();

    private CreateForecastCommandHandler CreateHandler()
    {
        return new CreateForecastCommandHandler(_forecastRepository, _locationRepository, _unitOfWork, _timeProvider,
            _logger);
    }

    [Fact]
    public async Task HandleRequest_WithFoundLocation_InsertsAndReturnsForecast()
    {
        // Arrange
        _locationRepository.FindAsync(1).Returns(TestLocation.Default);
        var handler = CreateHandler();

        var forecastDate = DateOnly.FromDateTime(s_currentDateTime.AddDays(1));
        var expectedForecast =
            new Forecast(forecastDate, TestLocation.Default, DateOnly.FromDateTime(s_currentDateTime));
        expectedForecast.DefinePrecipitationForecast(10, 15.5);

        // Act
        var command =
            new CreateForecastCommand { Date = forecastDate, Hour = 10, LocationId = 1, Precipitation = 15.5 };
        var result = await handler.HandleAsync(command);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBeEquivalentTo(expectedForecast.ToDto());

        _forecastRepository.Received(1).Add(Arg.Is<Forecast>(f => f.Date == forecastDate &&
                                                                  f.LocationId == TestLocation.Default.Id));
        await _unitOfWork.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task HandleRequest_WithBothPrecipitationAndTemperature_DefinesBothForecasts()
    {
        // Arrange
        _locationRepository.FindAsync(1).Returns(TestLocation.Default);
        var handler = CreateHandler();

        var forecastDate = DateOnly.FromDateTime(s_currentDateTime.AddDays(1));
        var expectedForecast =
            new Forecast(forecastDate, TestLocation.Default, DateOnly.FromDateTime(s_currentDateTime));
        expectedForecast.DefinePrecipitationForecast(10, 15.5);
        expectedForecast.DefineTemperatureForecast(10, 25.5);

        // Act
        var command = new CreateForecastCommand
        {
            Date = forecastDate,
            Hour = 10,
            LocationId = 1,
            Precipitation = 15.5,
            Temperature = 25.5
        };
        var result = await handler.HandleAsync(command);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBeEquivalentTo(expectedForecast.ToDto());

        _forecastRepository.Received(1).Add(Arg.Is<Forecast>(f => f.Date == forecastDate &&
                                                                  f.LocationId == TestLocation.Default.Id));
        await _unitOfWork.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task HandleRequest_WithExistingForecast_UpdatesExistingInsteadOfCreating()
    {
        // Arrange
        var forecastDate = DateOnly.FromDateTime(s_currentDateTime.AddDays(1));
        var existingForecast =
            new Forecast(forecastDate, TestLocation.Default, DateOnly.FromDateTime(s_currentDateTime));

        _locationRepository.FindAsync(1).Returns(TestLocation.Default);
        _forecastRepository.GetByDateForLocationAsync(1, forecastDate).Returns(existingForecast);
        var handler = CreateHandler();

        // Act
        var command = new CreateForecastCommand
        {
            Date = forecastDate,
            Hour = 10,
            LocationId = 1,
            Precipitation = 15.5
        };
        var result = await handler.HandleAsync(command);

        // Assert
        result.IsSuccess.ShouldBeTrue();

        _forecastRepository.Received(0).Add(Arg.Any<Forecast>());
        await _unitOfWork.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task HandleRequest_WithTemperature_DefinesTemperatureForecast()
    {
        // Arrange
        _locationRepository.FindAsync(1).Returns(TestLocation.Default);
        var handler = CreateHandler();

        var forecastDate = DateOnly.FromDateTime(s_currentDateTime.AddDays(1));
        var expectedForecast =
            new Forecast(forecastDate, TestLocation.Default, DateOnly.FromDateTime(s_currentDateTime));
        expectedForecast.DefineTemperatureForecast(10, 25.5);

        // Act
        var command =
            new CreateForecastCommand { Date = forecastDate, Hour = 10, LocationId = 1, Temperature = 25.5 };
        var result = await handler.HandleAsync(command);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBeEquivalentTo(expectedForecast.ToDto());

        _forecastRepository.Received(1).Add(Arg.Is<Forecast>(f => f.Date == forecastDate &&
                                                                  f.LocationId == TestLocation.Default.Id));
        await _unitOfWork.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task HandleRequest_WithNotFoundLocation_ReturnsErrorResult()
    {
        // Arrange
        _locationRepository.FindAsync(1).Returns((Location?)null);
        var handler = CreateHandler();

        var forecastDate = DateOnly.FromDateTime(s_currentDateTime.AddDays(1));

        // Act
        var command =
            new CreateForecastCommand { Date = forecastDate, Hour = 10, LocationId = 1, Precipitation = 15.5 };
        var result = await handler.HandleAsync(command);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Error.Type.ShouldBe(ErrorType.DependencyNotFound);
        result.Error.Metadata.ShouldNotBeNull();
        result.Error.Metadata.ShouldContainKey("missingIdentifier");
        result.Error.Metadata["missingIdentifier"].ShouldBe("Location");

        _forecastRepository.Received(0).Add(Arg.Any<Forecast>());
        await _unitOfWork.Received(0).SaveChangesAsync();
    }
}
