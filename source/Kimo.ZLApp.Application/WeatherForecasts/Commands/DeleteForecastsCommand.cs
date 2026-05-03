using System.ComponentModel.DataAnnotations;
using Kimo.ZLApp.Application.Common.RequestPipelines;
using Kimo.ZLApp.Application.Common.Results;

namespace Kimo.ZLApp.Application.WeatherForecasts.Commands;

/// <summary>
///     Löscht einen oder mehrere Wettervorhersagen.
/// </summary>
public sealed record DeleteForecastsCommand : IRequest<Result>
{
    /// <summary>
    ///     Die IDs der Vorhersagen.
    /// </summary>
    [Required]
    public required ICollection<int> ForecastIds { get; init; }
}