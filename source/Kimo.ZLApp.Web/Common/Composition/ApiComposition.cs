using System.Diagnostics.CodeAnalysis;
using Asp.Versioning;

namespace Kimo.ZLApp.Web.Common.Composition;

[ExcludeFromCodeCoverage]
internal static class ApiComposition
{
    public static IServiceCollection ConfigureApi(this IServiceCollection services)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
            .AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            })
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

        services.AddControllers();

        return services;
    }

    public static IEndpointRouteBuilder ConfigureApi(this IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapControllers();
        routeBuilder.MapFallbackToFile("index.html");

        return routeBuilder;
    }
}