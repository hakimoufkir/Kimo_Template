using Hangfire;
using Kimo.ZLApp.Application.Common.RequestPipelines;
using Kimo.ZLApp.Application.Common.Results;
using Kimo.ZLApp.Application.WeatherForecasts.Commands;
using Kimo.ZLApp.Infrastructure.Jobs.Common;

namespace Kimo.ZLApp.Infrastructure.Jobs.WeatherForecasts;

[AutomaticRetry(Attempts = 5)]
[FailOnError]
public class ForecastCleanupJob(IRequestExecutor requestExecutor)
{
    private const int MaxForecastLifetimeInDays = 30;

    public async Task<Result> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        return await requestExecutor.ExecuteAsync(
            new DeleteForecastsBeforeDateCommand { CutoffDays = MaxForecastLifetimeInDays }, cancellationToken);
    }
}
