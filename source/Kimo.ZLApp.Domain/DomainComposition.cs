using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Kimo.ZLApp.Domain;

[ExcludeFromCodeCoverage]
public static class DomainComposition
{
    public static IServiceCollection ConfigureDomainServices(this IServiceCollection services,
        IConfiguration configuration, IHostEnvironment environment)
    {
        return services;
    }
}