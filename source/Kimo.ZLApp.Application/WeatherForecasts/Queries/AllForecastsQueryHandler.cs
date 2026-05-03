using Microsoft.Extensions.Logging;
using Kimo.ZLApp.Application.Common.Filtering.Responses;
using Kimo.ZLApp.Application.Common.RequestPipelines;
using Kimo.ZLApp.Application.Common.Results;
using Kimo.ZLApp.Application.WeatherForecasts.Mappers;
using Kimo.ZLApp.Application.WeatherForecasts.Models;

namespace Kimo.ZLApp.Application.WeatherForecasts.Queries;

public sealed class AllForecastsQueryHandler(IForecastRepository repository, ILogger<AllForecastsQueryHandler> logger)
    : IRequestHandler<AllForecastsQuery, Result<PaginatedData<ForecastDto>>>
{
    public async Task<Result<PaginatedData<ForecastDto>>> HandleAsync(
        AllForecastsQuery request,
        CancellationToken cancellationToken = default
    )
    {
        var allForecasts =
            await repository.GetAllAsync(request.Pagination, request.Sorting, request.Filter, cancellationToken);

        logger.SuccessfullyLoadedForecasts(allForecasts.Page, allForecasts.PageSize, allForecasts.TotalRecords);

        return allForecasts.ToDto();
    }
}