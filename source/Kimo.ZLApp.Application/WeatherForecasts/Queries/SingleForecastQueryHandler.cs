using Microsoft.Extensions.Logging;
using Kimo.ZLApp.Application.Common.RequestPipelines;
using Kimo.ZLApp.Application.Common.Results;
using Kimo.ZLApp.Application.Common.Results.Error;
using Kimo.ZLApp.Application.WeatherForecasts.Mappers;
using Kimo.ZLApp.Application.WeatherForecasts.Models;

namespace Kimo.ZLApp.Application.WeatherForecasts.Queries;

public sealed class SingleForecastQueryHandler(
    IForecastRepository repository,
    ILogger<SingleForecastQueryHandler> logger)
    : IRequestHandler<SingleForecastQuery, Result<ForecastDto>>
{
    public async Task<Result<ForecastDto>> HandleAsync(
        SingleForecastQuery request,
        CancellationToken cancellationToken = default
    )
    {
        var forecast = await repository.FindAsync(request.Id, cancellationToken);

        if (forecast is null)
        {
            logger.ForecastNotFound(request.Id);
            return Error.NotFound();
        }

        logger.SuccessfullyLoadedForecast(forecast);
        return forecast.ToDto();
    }
}