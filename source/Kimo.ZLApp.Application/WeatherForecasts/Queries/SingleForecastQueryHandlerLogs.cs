using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Kimo.ZLApp.Domain.WeatherForecasts;

namespace Kimo.ZLApp.Application.WeatherForecasts.Queries;

[ExcludeFromCodeCoverage]
public static partial class SingleForecastQueryHandlerLogs
{
    [LoggerMessage(
        EventId = ApplicationLogEventIds.WeatherForecasts.Queries.SingleForecast.SuccessfullyLoaded,
        Level = LogLevel.Information,
        Message = "Successfully loaded forecast: {@Forecast}")]
    public static partial void SuccessfullyLoadedForecast(this ILogger<SingleForecastQueryHandler> logger,
        Forecast forecast);

    [LoggerMessage(
        EventId = ApplicationLogEventIds.WeatherForecasts.Queries.SingleForecast.NotFound,
        Level = LogLevel.Warning,
        Message = "Forecast with id {ForecastId} not found")]
    public static partial void ForecastNotFound(this ILogger<SingleForecastQueryHandler> logger, int forecastId);
}