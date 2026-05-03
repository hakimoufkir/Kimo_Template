using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Kimo.ZLApp.Application.Common.Composition;

namespace Kimo.ZLApp.Application;

[ExcludeFromCodeCoverage]
public static class AppComposition
{
    public static IServiceCollection ConfigureApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment
    )
    {
        return services.ConfigureServices()
            .ConfigureRequestPipelines();
    }
}