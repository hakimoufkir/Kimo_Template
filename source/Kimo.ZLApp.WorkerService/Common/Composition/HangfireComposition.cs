using System.Diagnostics.CodeAnalysis;
using Hangfire;
using Hangfire.Client;
using Hangfire.Common;
using Hangfire.Server;
using Hangfire.States;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Kimo.ZLApp.Infrastructure;
using Kimo.ZLApp.WorkerService.Common.Logging;

namespace Kimo.ZLApp.WorkerService.Common.Composition;

[ExcludeFromCodeCoverage]
internal static class HangfireComposition
{
    public static IServiceCollection ConfigureHangfireServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        var hangfireConnectionString = configuration.GetConnectionString(ConnectionStringName.Hangfire);
        services.AddHangfire((s, c) =>
        {
            c.UseSqlServerStorage(hangfireConnectionString);
            c.UseSerilogLogProvider();
            c.UseFilter(new HangfireJobLoggerAttribute(s.GetRequiredService<ILogger<HangfireJobLoggerAttribute>>()));
        });

        services.AddHangfireServer();

        services.TryAddSingleton<IBackgroundJobStateChanger, BackgroundJobStateChanger>();
        services.TryAddSingleton<IBackgroundJobFactory, BackgroundJobFactory>();
        services.TryAddSingleton<IBackgroundJobPerformer>(x => new HangfireLogContextJobPerformer(
            new BackgroundJobPerformer(
                x.GetRequiredService<IJobFilterProvider>(),
                x.GetRequiredService<JobActivator>(),
                TaskScheduler.Default)));

        return services;
    }
}