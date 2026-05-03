using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace Kimo.ZLApp.Application.WeatherForecasts.Commands;

[ExcludeFromCodeCoverage]
public static partial class DeleteForecastsBeforeDateCommandHandlerLogs
{
    [LoggerMessage(
        EventId = ApplicationLogEventIds.WeatherForecasts.Commands.DeleteForecastBeforeDate.DeletedSuccessfully,
        Level = LogLevel.Information,
        Message = "Successfully deleted {DeletedForecasts} forecasts before {CutoffDate}")]
    public static partial void DeletedSuccessfully(this ILogger<DeleteForecastsBeforeDateCommandHandler> logger,
        DateOnly cutoffDate, int deletedForecasts);
}