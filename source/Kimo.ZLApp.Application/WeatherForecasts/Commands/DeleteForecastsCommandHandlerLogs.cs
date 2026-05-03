using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace Kimo.ZLApp.Application.WeatherForecasts.Commands;

[ExcludeFromCodeCoverage]
public static partial class DeleteForecastCommandHandlerLogs
{
    [LoggerMessage(
        EventId = ApplicationLogEventIds.WeatherForecasts.Commands.DeleteForecast.ForecastNotFound,
        Level = LogLevel.Warning,
        Message = "The forecast with id {ForecastId} could not be found")]
    public static partial void ForecastNotFound(this ILogger<DeleteForecastsCommandHandler> logger, int forecastId);

    [LoggerMessage(
        EventId = ApplicationLogEventIds.WeatherForecasts.Commands.DeleteForecast.ForecastRemoved,
        Level = LogLevel.Debug,
        Message = "The forecast with id {ForecastId} has been removed")]
    public static partial void ForecastRemoved(this ILogger<DeleteForecastsCommandHandler> logger, int forecastId);

    [LoggerMessage(
        EventId = ApplicationLogEventIds.WeatherForecasts.Commands.DeleteForecast.DeletedSuccessfully,
        Level = LogLevel.Information,
        Message =
            "Successfully deleted {TotalForecasts} out of {DeletedForecasts} forecasts. Not found forecasts: {NotFoundForecasts}")]
    public static partial void DeletedSuccessfully(this ILogger<DeleteForecastsCommandHandler> logger,
        int totalForecasts, int deletedForecasts, int notFoundForecasts);
}