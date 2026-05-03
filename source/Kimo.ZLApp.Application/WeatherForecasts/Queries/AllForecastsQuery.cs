using System.ComponentModel.DataAnnotations;
using Kimo.ZLApp.Application.Common.Filtering.Requests;
using Kimo.ZLApp.Application.Common.Filtering.Responses;
using Kimo.ZLApp.Application.Common.Filtering.Validators;
using Kimo.ZLApp.Application.Common.RequestPipelines;
using Kimo.ZLApp.Application.Common.Results;
using Kimo.ZLApp.Application.WeatherForecasts.Models;
using Kimo.ZLApp.Domain.WeatherForecasts;

namespace Kimo.ZLApp.Application.WeatherForecasts.Queries;

/// <summary>
///     Returniert die gefilterte Liste aller Wettervorhersagen.
/// </summary>
public sealed record AllForecastsQuery : IRequest<Result<PaginatedData<ForecastDto>>>
{
    /// <summary>
    ///     Die Optionen für die Paginierung.
    /// </summary>
    [Required]
    public required PaginationOptions Pagination { get; init; }

    /// <summary>
    ///     Die Optionen für die Sortierung.
    /// </summary>
    [Required]
    public required SortingOptions Sorting { get; init; }

    /// <summary>
    ///     Die Optionen für die Filterung.
    /// </summary>
    [AllowedFilterFields(nameof(Forecast.LocationId), nameof(Forecast.Location), nameof(Forecast.Date))]
    public required FilterOptions? Filter { get; init; }
}