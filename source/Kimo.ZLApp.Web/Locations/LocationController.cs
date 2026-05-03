using Asp.Versioning;
using Kimo.ZLApp.Application.Common.RequestPipelines;
using Kimo.ZLApp.Application.Locations.Models;
using Kimo.ZLApp.Application.Locations.Queries;
using Kimo.ZLApp.Web.Common.RequestPipeline;

namespace Kimo.ZLApp.Web.Locations;

/// <summary>
///     Controller for managing locations.
/// </summary>
/// <param name="requestExecutor">Shared request pipeline used to execute commands and queries.</param>
[ApiController]
[ApiVersion(1)]
[Route("api/v{version:apiVersion}/locations")]
public class LocationController(IRequestExecutor requestExecutor)
{
    /// <summary>
    ///     Returns all locations.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <response code="200">Locations were loaded successfully.</response>
    /// <returns>List of locations.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public Task<ActionResult<List<LocationDto>>> Get(CancellationToken cancellationToken)
    {
        return requestExecutor.Handle(new AllLocationsQuery(), cancellationToken);
    }
}
