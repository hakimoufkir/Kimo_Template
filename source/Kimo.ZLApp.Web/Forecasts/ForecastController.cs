using Asp.Versioning;
using Kimo.ZLApp.Application.Common.Filtering.Responses;
using Kimo.ZLApp.Application.Common.RequestPipelines;
using Kimo.ZLApp.Application.WeatherForecasts.Commands;
using Kimo.ZLApp.Application.WeatherForecasts.Models;
using Kimo.ZLApp.Application.WeatherForecasts.Queries;
using Kimo.ZLApp.Web.Common.Filtering;
using Kimo.ZLApp.Web.Common.RequestPipeline;

namespace Kimo.ZLApp.Web.Forecasts;

/// <summary>
///     Controller for managing weather forecasts.
/// </summary>
/// <param name="requestExecutor">Shared request pipeline used to execute commands and queries.</param>
[ApiController]
[ApiVersion(1)]
[Route("api/v{version:apiVersion}/forecasts")]
public class ForecastController(IRequestExecutor requestExecutor)
{
    /// <summary>
    ///     Returns all weather forecasts.
    /// </summary>
    /// <param name="queryParameters">Query parameters used for filtering, pagination, and sorting.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <response code="200">Forecasts were loaded successfully.</response>
    /// <returns>Paginated forecast data.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public Task<ActionResult<PaginatedData<ForecastDto>>> Get(
        [FromQuery] QueryParameters queryParameters,
        CancellationToken cancellationToken
    )
    {
        return requestExecutor.Handle(
            new AllForecastsQuery
            {
                Pagination = queryParameters.GetPaginationOptions(),
                Sorting = queryParameters.GetSortingOptions(),
                Filter = queryParameters.GetFilterOptions()
            },
            cancellationToken);
    }

    /// <summary>
    ///     Returns a single weather forecast by id.
    /// </summary>
    /// <param name="id">Forecast id.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <response code="200">The forecast was loaded successfully.</response>
    /// <response code="404">No forecast with the given id was found.</response>
    /// <returns>The matching forecast.</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<ActionResult<ForecastDto>> Get(
        [FromRoute] int id,
        CancellationToken cancellationToken
    )
    {
        return requestExecutor.Handle(new SingleForecastQuery { Id = id }, cancellationToken);
    }

    /// <summary>
    ///     Deletes one or more weather forecasts by id.
    /// </summary>
    /// <param name="id">One or more forecast ids to delete.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <response code="204">The forecast(s) were deleted successfully.</response>
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public Task<ActionResult> Delete(
        [FromQuery] ICollection<int> id,
        CancellationToken cancellationToken
    )
    {
        return requestExecutor.Handle(new DeleteForecastsCommand { ForecastIds = id }, cancellationToken);
    }

    /// <summary>
    ///     Creates a new weather forecast from the supplied request body.
    /// </summary>
    /// <param name="command">Forecast input data.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <response code="201">The forecast was created successfully.</response>
    /// <response code="400">Validation failed.</response>
    /// <response code="403">The caller is authenticated but not allowed to create forecasts.</response>
    /// <remarks>
    ///     Required permission: <c>forecast.create</c>
    /// </remarks>
    /// <returns>The created forecast.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public Task<ActionResult<ForecastDto>> Post(
        [FromBody] CreateForecastCommand command,
        CancellationToken cancellationToken
    )
    {
        return requestExecutor.Handle(command, cancellationToken);
    }
}
