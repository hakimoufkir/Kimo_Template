using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Kimo.ZLApp.Application.Locations;
using Kimo.ZLApp.Application.WeatherForecasts;
using Kimo.ZLApp.Infrastructure.Locations;
using Kimo.ZLApp.Infrastructure.WeatherForecasts;

namespace Kimo.ZLApp.Infrastructure.Common.Composition;

[ExcludeFromCodeCoverage]
internal static class RepositoriesComposition
{
    public static IServiceCollection ConfigureRepositories(this IServiceCollection services)
    {
        return services.AddScoped<IForecastRepository, ForecastRepository>()
            .AddScoped<ILocationRepository, LocationRepository>();
    }
}