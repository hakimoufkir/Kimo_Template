using Kimo.ZLApp.Application.Locations.Models;

namespace Kimo.ZLApp.Application.WeatherForecasts.Models;

/// <summary>
///     Das Datenobjekt für eine Wettervorhersage.
/// </summary>
/// <param name="Id">Die ID der Vorhersage.</param>
/// <param name="Date">Das Datum, wann die Vorhersage gültig ist.</param>
/// <param name="Location">Der Standort der Vorhersage.</param>
/// <param name="TemperatureData">Die Sammlung aller Temperaturdaten für den Tag.</param>
/// <param name="PrecipitationData">Die Sammlung aller Niederschlagsdaten für den Tag.</param>
public sealed record ForecastDto(
    int Id,
    string Date,
    LocationDto Location,
    TemperatureDto[] TemperatureData,
    PrecipitationDto[] PrecipitationData
);