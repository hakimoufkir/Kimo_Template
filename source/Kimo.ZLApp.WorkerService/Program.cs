using Hangfire;
using Kimo.ZLApp.WorkerService;
using Kimo.ZLApp.WorkerService.Common.Composition;
using Serilog;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.ConfigureServices(builder.Configuration, builder.Environment)
    .ConfigureLogger(builder.Configuration);

builder.Logging.ConfigureLogger();

try
{
    Log.Information("Building worker service host...");
    var host = builder.Build();

    using (var scope = host.Services.CreateScope())
    {
        var recurringJobManager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

        Log.Information("Registering Hangfire recurring jobs...");
        HangfireJobRegistration.RegisterGlobalRecurringJobs(recurringJobManager, configuration);
    }

    Log.Information("Starting host...");
    host.Run();

    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Worker service host terminated unexpectedly");
    return 1;
}
finally
{
    Log.Verbose("Worker service host exited. Closing and flushing the logger...");
    Log.CloseAndFlush();
}