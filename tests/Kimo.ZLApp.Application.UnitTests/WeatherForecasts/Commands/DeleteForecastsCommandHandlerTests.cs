using Microsoft.Extensions.Logging;
using Kimo.ZLApp.Application.Common;
using Kimo.ZLApp.Application.Common.Results.ResultModels;
using Kimo.ZLApp.Application.WeatherForecasts;
using Kimo.ZLApp.Application.WeatherForecasts.Commands;
using Kimo.ZLApp.Domain.WeatherForecasts;
using Kimo.ZLApp.Tests.Common.Forecasts;
using Kimo.ZLApp.Tests.Common.Locations;

namespace Kimo.ZLApp.Application.UnitTests.WeatherForecasts.Commands;

public class DeleteForecastCommandHandlerTests
{
    private readonly IForecastRepository _forecastRepository = Substitute.For<IForecastRepository>();

    private readonly ILogger<DeleteForecastsCommandHandler> _logger =
        Substitute.For<ILogger<DeleteForecastsCommandHandler>>();

    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();

    private DeleteForecastsCommandHandler CreateHandler()
    {
        return new DeleteForecastsCommandHandler(_forecastRepository, _unitOfWork, _logger);
    }

    [Fact]
    public async Task HandleAsync_WithSingleExistingForecast_DeletesSuccessfully()
    {
        // Arrange
        var forecast = TestForecast.Default(TestLocation.Default);
        _forecastRepository.FindAsync(1, Arg.Any<CancellationToken>())
            .Returns(forecast);

        var handler = CreateHandler();
        var command = new DeleteForecastsCommand { ForecastIds = [1] };

        // Act
        var result = await handler.HandleAsync(command);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        var deletedResult = result.ShouldBeOfType<DeletedResult>();
        deletedResult.Requested.ShouldBe(1);
        deletedResult.Successful.ShouldBe(1);
        deletedResult.NotFound.ShouldBe(0);

        _forecastRepository.Received(1).Remove(forecast);
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_WithMultipleExistingForecasts_DeletesAllSuccessfully()
    {
        // Arrange
        var forecast1 = TestForecast.Default(TestLocation.Default);
        var forecast2 = TestForecast.Alternative(TestLocation.Default);

        _forecastRepository.FindAsync(1, Arg.Any<CancellationToken>()).Returns(forecast1);
        _forecastRepository.FindAsync(2, Arg.Any<CancellationToken>()).Returns(forecast2);

        var handler = CreateHandler();
        var command = new DeleteForecastsCommand { ForecastIds = [1, 2] };

        // Act
        var result = await handler.HandleAsync(command);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        var deletedResult = result.ShouldBeOfType<DeletedResult>();
        deletedResult.Requested.ShouldBe(2);
        deletedResult.Successful.ShouldBe(2);
        deletedResult.NotFound.ShouldBe(0);

        _forecastRepository.Received(1).Remove(forecast1);
        _forecastRepository.Received(1).Remove(forecast2);
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_WithNonExistingForecast_ReturnsNotFoundCount()
    {
        // Arrange
        _forecastRepository.FindAsync(1, Arg.Any<CancellationToken>())
            .Returns((Forecast?)null);

        var handler = CreateHandler();
        var command = new DeleteForecastsCommand { ForecastIds = [1] };

        // Act
        var result = await handler.HandleAsync(command);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        var deletedResult = result.ShouldBeOfType<DeletedResult>();
        deletedResult.Requested.ShouldBe(1);
        deletedResult.Successful.ShouldBe(0);
        deletedResult.NotFound.ShouldBe(1);

        _forecastRepository.Received(0).Remove(Arg.Any<Forecast>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_WithMixedExistingAndNonExisting_DeletesOnlyExisting()
    {
        // Arrange
        var forecast1 = TestForecast.Default(TestLocation.Default);
        var forecast3 = TestForecast.Alternative(TestLocation.Default);

        _forecastRepository.FindAsync(1, Arg.Any<CancellationToken>()).Returns(forecast1);
        _forecastRepository.FindAsync(2, Arg.Any<CancellationToken>()).Returns((Forecast?)null);
        _forecastRepository.FindAsync(3, Arg.Any<CancellationToken>()).Returns(forecast3);

        var handler = CreateHandler();
        var command = new DeleteForecastsCommand { ForecastIds = [1, 2, 3] };

        // Act
        var result = await handler.HandleAsync(command);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        var deletedResult = result.ShouldBeOfType<DeletedResult>();
        deletedResult.Requested.ShouldBe(3);
        deletedResult.Successful.ShouldBe(2);
        deletedResult.NotFound.ShouldBe(1);

        _forecastRepository.Received(1).Remove(forecast1);
        _forecastRepository.Received(1).Remove(forecast3);
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_WithEmptyForecastIds_SavesChangesWithoutDeletions()
    {
        // Arrange
        var handler = CreateHandler();
        var command = new DeleteForecastsCommand { ForecastIds = [] };

        // Act
        var result = await handler.HandleAsync(command);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        var deletedResult = result.ShouldBeOfType<DeletedResult>();
        deletedResult.Requested.ShouldBe(0);
        deletedResult.Successful.ShouldBe(0);
        deletedResult.NotFound.ShouldBe(0);

        _forecastRepository.Received(0).Remove(Arg.Any<Forecast>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
