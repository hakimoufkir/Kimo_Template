using Microsoft.Extensions.Logging;
using Kimo.ZLApp.Application.Common;
using Kimo.ZLApp.Application.Common.RequestPipelines;
using Kimo.ZLApp.Application.Common.Results;
using Kimo.ZLApp.Application.Common.Results.ResultModels;

namespace Kimo.ZLApp.Application.WeatherForecasts.Commands;

public sealed class DeleteForecastsCommandHandler(
    IForecastRepository forecastRepo,
    IUnitOfWork unitOfWork,
    ILogger<DeleteForecastsCommandHandler> logger
)
    : IRequestHandler<DeleteForecastsCommand, Result>
{
    public async Task<Result> HandleAsync(
        DeleteForecastsCommand request,
        CancellationToken cancellationToken = default
    )
    {
        var notFound = 0;
        var successful = 0;

        foreach (var forecastId in request.ForecastIds)
        {
            var foundForecast = await forecastRepo.FindAsync(forecastId, cancellationToken);
            if (foundForecast is null)
            {
                notFound++;
                logger.ForecastNotFound(forecastId);

                continue;
            }

            forecastRepo.Remove(foundForecast);
            logger.ForecastRemoved(forecastId);

            successful++;
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        logger.DeletedSuccessfully(request.ForecastIds.Count, successful, notFound);

        return new DeletedResult(request.ForecastIds.Count, successful, notFound);
    }
}