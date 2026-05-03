using System.ComponentModel.DataAnnotations;
using Kimo.ZLApp.Application.Common.RequestPipelines;
using Kimo.ZLApp.Application.Common.Results;
using Kimo.ZLApp.Application.WeatherForecasts.Models;
using Kimo.ZLApp.Application.WeatherForecasts.Validators;

namespace Kimo.ZLApp.Application.WeatherForecasts.Commands;

/// <summary>
///     Erstellt einen neuen Eintrag für eine Wettervorhersage.
///     Falls ein Standort bereits eine Vorhersage für den Tag hat, wird ein neuer Eintrag für die Uhrzeit hinzugefügt.
/// </summary>
public sealed record CreateForecastCommand : IRequest<Result<ForecastDto>>
{
    /// <summary>
    ///     Das Datum des Eintrags.
    /// </summary>
    [Required]
    public required DateOnly Date { get; init; }

    /// <summary>
    ///     Die ID des Standorts.
    /// </summary>
    [Required]
    [Range(1, int.MaxValue)]
    public required int LocationId { get; init; }

    /// <summary>
    ///     Die Stunde der Vorhersage.
    /// </summary>
    [Required]
    [IsEven]
    [Range(0, 23)]
    public required int Hour { get; init; }

    /// <summary>
    ///     Die Temperatur für den entsprechenden Zeitpunkt.
    /// </summary>
    [Range(-100, 100)]
    public double? Temperature { get; init; }

    /// <summary>
    ///     Die Niederschlagswahrscheinlichkeit für den entsprechenden Zeitpunkt.
    /// </summary>
    [Range(0, 100)]
    public double? Precipitation { get; init; }
}