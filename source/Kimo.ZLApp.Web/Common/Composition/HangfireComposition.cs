using System.Diagnostics.CodeAnalysis;
using Hangfire;
using Hangfire.SqlServer;

namespace Kimo.ZLApp.Web.Common.Composition;

[ExcludeFromCodeCoverage]
internal static class HangfireComposition
{
    public static IServiceCollection ConfigureHangfire(this IServiceCollection services, IConfiguration configuration)
    {
        var hangfireConnectionString = configuration.GetConnectionString("Hangfire");

        services.AddHangfire(x => x
            .UseSqlServerStorage(hangfireConnectionString,
                new SqlServerStorageOptions { QueuePollInterval = TimeSpan.FromSeconds(10) }));

        return services;
    }

    public static IApplicationBuilder ConfigureHangfire(this IApplicationBuilder application)
    {
        return application.UseHangfireDashboard("/jobs",
            new DashboardOptions
            {
                IgnoreAntiforgeryToken = true
            });
    }
}
