namespace Kimo.ZLApp.Application.WeatherForecasts.Models;

/// <summary>
///     Das Datenobjekt für die Niederschlagswahrscheinlichkeit.
/// </summary>
/// <param name="Hour">Die Stunde der Vorhersage.</param>
/// <param name="Value">Die Niederschlagswahrscheinlichkeit in Prozent.</param>
public sealed record PrecipitationDto(int Hour, double Value);