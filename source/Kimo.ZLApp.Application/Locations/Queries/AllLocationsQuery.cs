using Kimo.ZLApp.Application.Common.RequestPipelines;
using Kimo.ZLApp.Application.Common.Results;
using Kimo.ZLApp.Application.Locations.Models;

namespace Kimo.ZLApp.Application.Locations.Queries;

/// <summary>
///     Lädt alle Standorte.
/// </summary>
public sealed record AllLocationsQuery : IRequest<Result<List<LocationDto>>>;