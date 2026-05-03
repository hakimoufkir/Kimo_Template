using System.Diagnostics.CodeAnalysis;
using Kimo.ZLApp.Application;
using Kimo.ZLApp.Domain;
using Kimo.ZLApp.Infrastructure;

namespace Kimo.ZLApp.Web.Common.Composition;

[ExcludeFromCodeCoverage]
internal static class ServicesComposition
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration,
        IHostEnvironment environment)
    {
        return services.ConfigureDomainServices(configuration, environment)
            .ConfigureApplicationServices(configuration, environment)
            .ConfigureInfrastructureServices(configuration, environment);
    }
}