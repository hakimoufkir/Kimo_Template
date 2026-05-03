using System.ComponentModel.DataAnnotations;
using Kimo.ZLApp.Application.Common.RequestPipelines;
using Kimo.ZLApp.Application.Common.Results;

namespace Kimo.ZLApp.Application.WeatherForecasts.Commands;

/// <summary>
///     Löscht alle Vorhersagen, die älter als X Tage sind.
/// </summary>
public sealed record DeleteForecastsBeforeDateCommand : IRequest<Result>
{
    /// <summary>
    ///     Die Anzahl an Tagen, ab wann die Einträge gelöscht werden sollen.
    /// </summary>
    [Required]
    [Range(0, int.MaxValue)]
    public required int CutoffDays { get; init; }
}