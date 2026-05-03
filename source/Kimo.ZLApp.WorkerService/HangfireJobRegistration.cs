using System.Diagnostics.CodeAnalysis;
using Hangfire;
using Kimo.ZLApp.Infrastructure.Jobs.WeatherForecasts;

namespace Kimo.ZLApp.WorkerService;

[ExcludeFromCodeCoverage]
internal static class HangfireJobRegistration
{
    public static void RegisterGlobalRecurringJobs(IRecurringJobManager recurringJobManager,
        IConfiguration configuration)
    {
        recurringJobManager.AddOrUpdate<ForecastCleanupJob>(
            nameof(ForecastCleanupJob),
            x => x.ExecuteAsync(CancellationToken.None),
            configuration["JobConfiguration:ForecastCleanupJobTrigger"],
            new RecurringJobOptions { TimeZone = TimeZoneInfo.Local });
    }
}