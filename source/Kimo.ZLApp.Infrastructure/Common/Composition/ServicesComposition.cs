using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;

namespace Kimo.ZLApp.Infrastructure.Common.Composition;

[ExcludeFromCodeCoverage]
internal static class ServicesComposition
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        return services.AddSingleton(TimeProvider.System);
    }
}