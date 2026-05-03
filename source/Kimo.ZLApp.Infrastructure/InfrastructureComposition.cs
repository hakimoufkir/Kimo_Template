using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Kimo.ZLApp.Infrastructure.Common.Composition;

namespace Kimo.ZLApp.Infrastructure;

[ExcludeFromCodeCoverage]
public static class InfrastructureComposition
{
    public static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration, IHostEnvironment environment)
    {
        return services.ConfigureServices()
            .ConfigureDatabaseServices(configuration)
            .ConfigureRepositories();
    }
}
