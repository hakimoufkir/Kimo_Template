using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace Kimo.ZLApp.Application.WeatherForecasts.Commands;

[ExcludeFromCodeCoverage]
public static partial class CreateForecastCommandHandlerLogs
{
    [LoggerMessage(
        EventId = ApplicationLogEventIds.WeatherForecasts.Commands.CreateForecast.LocationNotFound,
        Level = LogLevel.Warning,
        Message = "The location with id {LocationId} could not be found")]
    public static partial void LocationNotFound(this ILogger<CreateForecastCommandHandler> logger, int locationId);

    [LoggerMessage(
        EventId = ApplicationLogEventIds.WeatherForecasts.Commands.CreateForecast.CreateNewForecast,
        Level = LogLevel.Information,
        Message = "Created new forecast with id {LocationId} for date {Date}")]
    public static partial void CreateNewForecast(this ILogger<CreateForecastCommandHandler> logger, int locationId,
        DateOnly date);


    [LoggerMessage(
        EventId = ApplicationLogEventIds.WeatherForecasts.Commands.CreateForecast.AddingPrecipitation,
        Level = LogLevel.Debug,
        Message = "Added precipitation {Precipitation} for hour {Hour}")]
    public static partial void AddingPrecipitation(this ILogger<CreateForecastCommandHandler> logger, int hour,
        double precipitation);

    [LoggerMessage(
        EventId = ApplicationLogEventIds.WeatherForecasts.Commands.CreateForecast.AddingTemperature,
        Level = LogLevel.Debug,
        Message = "Added temperature {Temperature} for hour {Hour}")]
    public static partial void AddingTemperature(this ILogger<CreateForecastCommandHandler> logger, int hour,
        double temperature);

    [LoggerMessage(
        EventId = ApplicationLogEventIds.WeatherForecasts.Commands.CreateForecast.SuccessfullyCreated,
        Level = LogLevel.Error,
        Message = "Successfully created forecast with id {ForecastId}")]
    public static partial void CreatedSuccessfully(this ILogger<CreateForecastCommandHandler> logger, int forecastId);
}