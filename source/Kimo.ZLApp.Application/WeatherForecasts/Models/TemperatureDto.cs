namespace Kimo.ZLApp.Application.WeatherForecasts.Models;

/// <summary>
///     Das Datenobjekt für die Temperatur.
/// </summary>
/// <param name="Hour">Die Stunde der Vorhersage.</param>
/// <param name="Value">Die Temperatur in Grad Celsius.</param>
public sealed record TemperatureDto(int Hour, double Value);