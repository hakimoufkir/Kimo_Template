using System.ComponentModel.DataAnnotations;
using Kimo.ZLApp.Application.Common.RequestPipelines;
using Kimo.ZLApp.Application.Common.Results;
using Kimo.ZLApp.Application.WeatherForecasts.Models;

namespace Kimo.ZLApp.Application.WeatherForecasts.Queries;

/// <summary>
///     Returniert eine einzelne Wettervorhersage.
/// </summary>
public sealed record SingleForecastQuery : IRequest<Result<ForecastDto>>
{
    /// <summary>
    ///     Die ID der angeforderten Vorhersage.
    /// </summary>
    [Required]
    [Range(1, int.MaxValue)]
    public required int Id { get; init; }
}