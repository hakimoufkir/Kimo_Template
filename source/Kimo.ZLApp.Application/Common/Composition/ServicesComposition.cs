using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;

namespace Kimo.ZLApp.Application.Common.Composition;

[ExcludeFromCodeCoverage]
internal static class ServicesComposition
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        return services;
    }
}