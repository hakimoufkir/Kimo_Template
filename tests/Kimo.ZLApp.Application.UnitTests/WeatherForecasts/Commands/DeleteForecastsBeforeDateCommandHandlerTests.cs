using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Time.Testing;
using Kimo.ZLApp.Application.WeatherForecasts;
using Kimo.ZLApp.Application.WeatherForecasts.Commands;

namespace Kimo.ZLApp.Application.UnitTests.WeatherForecasts.Commands;

public class DeleteForecastsBeforeDateCommandHandlerTests
{
    private static readonly DateTime s_currentDateTime = new(2026, 2, 1, 15, 20, 30, DateTimeKind.Local);
    private readonly IForecastRepository _forecastRepository = Substitute.For<IForecastRepository>();

    private readonly ILogger<DeleteForecastsBeforeDateCommandHandler> _logger =
        Substitute.For<ILogger<DeleteForecastsBeforeDateCommandHandler>>();

    private readonly TimeProvider _timeProvider = new FakeTimeProvider(s_currentDateTime);

    private DeleteForecastsBeforeDateCommandHandler CreateHandler()
    {
        return new DeleteForecastsBeforeDateCommandHandler(_forecastRepository, _timeProvider, _logger);
    }

    [Fact]
    public async Task HandleRequest_ShouldCallDeleteWithDateInPast()
    {
        // Arrange
        var handler = CreateHandler();
        var command = new DeleteForecastsBeforeDateCommand { CutoffDays = 30 };
        var dateInPast = new DateOnly(2026, 1, 2);

        // Act
        await handler.HandleAsync(command);

        // Assert
        await _forecastRepository.Received(1).RemoveAllBeforeDateAsync(dateInPast);
    }
}
