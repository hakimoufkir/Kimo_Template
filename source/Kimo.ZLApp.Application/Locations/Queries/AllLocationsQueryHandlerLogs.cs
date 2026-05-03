using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace Kimo.ZLApp.Application.Locations.Queries;

[ExcludeFromCodeCoverage]
public static partial class AllLocationsQueryHandlerLogs
{
    [LoggerMessage(
        EventId = ApplicationLogEventIds.Locations.Queries.AllLocations.SuccessfullyLoaded,
        Level = LogLevel.Information,
        Message = "Successfully loaded {NumberOfLocations} locations")]
    public static partial void SuccessfullyLoadedLocations(this ILogger<AllLocationsQueryHandler> logger,
        int numberOfLocations);
}