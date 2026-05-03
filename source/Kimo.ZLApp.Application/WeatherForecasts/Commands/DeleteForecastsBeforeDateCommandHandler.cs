using Microsoft.Extensions.Logging;
using Kimo.ZLApp.Application.Common.RequestPipelines;
using Kimo.ZLApp.Application.Common.Results;
using Kimo.ZLApp.Application.Common.Results.ResultModels;
using Kimo.ZLApp.Application.Common.TimeProvider;

namespace Kimo.ZLApp.Application.WeatherForecasts.Commands;

public sealed class DeleteForecastsBeforeDateCommandHandler(
    IForecastRepository forecastRepo,
    TimeProvider timeProvider,
    ILogger<DeleteForecastsBeforeDateCommandHandler> logger
)
    : IRequestHandler<DeleteForecastsBeforeDateCommand, Result>
{
    public async Task<Result> HandleAsync(
        DeleteForecastsBeforeDateCommand request,
        CancellationToken cancellationToken = default
    )
    {
        var cutoffDate = timeProvider.GetLocalToday().AddDays(-request.CutoffDays);
        var deletedEntries = await forecastRepo.RemoveAllBeforeDateAsync(cutoffDate, cancellationToken);

        logger.DeletedSuccessfully(cutoffDate, deletedEntries);

        return new DeletedResult(null, deletedEntries, 0);
    }
}