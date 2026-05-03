using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Serilog;
using Serilog.Exceptions;
using Serilog.Exceptions.Core;
using Serilog.Exceptions.EntityFrameworkCore.Destructurers;

namespace Kimo.ZLApp.WorkerService.Common.Composition;

[ExcludeFromCodeCoverage]
internal static class LoggerComposition
{
    public static IServiceCollection ConfigureLogger(this IServiceCollection services, IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithEnvironmentUserName()
            .Enrich.WithProcessId()
            .Enrich.WithProcessName()
            .Enrich.WithExceptionDetails(new DestructuringOptionsBuilder()
                .WithDefaultDestructurers()
                .WithDestructurers([new DbUpdateExceptionDestructurer()]))
            .WriteTo.Console(formatProvider: CultureInfo.InvariantCulture)
            .CreateLogger();

        services.AddSerilog(Log.Logger);
        return services;
    }

    public static ILoggingBuilder ConfigureLogger(this ILoggingBuilder loggingBuilder)
    {
        return loggingBuilder.AddSerilog(Log.Logger);
    }
}