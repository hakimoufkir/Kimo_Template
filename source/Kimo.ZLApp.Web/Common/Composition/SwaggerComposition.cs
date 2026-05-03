using System.Diagnostics.CodeAnalysis;
using Asp.Versioning.ApiExplorer;
using Kimo.ZLApp.Web.Common.Swagger;

namespace Kimo.ZLApp.Web.Common.Composition;

[ExcludeFromCodeCoverage]
internal static class SwaggerComposition
{
    public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer()
            .ConfigureOptions<ConfigureSwaggerOptions>()
            .ConfigureSwaggerServices();

        return services;
    }

    public static IApplicationBuilder ConfigureSwagger(this IApplicationBuilder application,
        IServiceProvider serviceProvider)
    {
        var provider = serviceProvider.GetRequiredService<IApiVersionDescriptionProvider>();
        return application.UseSwagger()
            .UseSwaggerUI(options =>
            {
                foreach (var desc in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint(
                        $"/swagger/{desc.GroupName}/swagger.json",
                        desc.GroupName.ToUpperInvariant());
                }
            });
    }
}