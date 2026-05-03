using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace Kimo.ZLApp.Application.WeatherForecasts.Queries;

[ExcludeFromCodeCoverage]
public static partial class AllForecastsQueryHandlerLogs
{
    [LoggerMessage(
        EventId = ApplicationLogEventIds.WeatherForecasts.Queries.AllForecasts.SuccessfullyLoaded,
        Level = LogLevel.Information,
        Message = "Successfully loaded {NumberOfForecasts}/{Take} forecasts for {Page}")]
    public static partial void SuccessfullyLoadedForecasts(this ILogger<AllForecastsQueryHandler> logger, int page,
        int take, int numberOfForecasts);
}