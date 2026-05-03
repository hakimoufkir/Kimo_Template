using System.Diagnostics.CodeAnalysis;
using Microsoft.OpenApi.Models;
using Kimo.ZLApp.Web.Common.Composition;
using Kimo.ZLApp.Web.Common.Swagger.Filters;
using Swashbuckle.AspNetCore.Filters;

namespace Kimo.ZLApp.Web.Common.Swagger;

[ExcludeFromCodeCoverage]
internal static class SwaggerConfiguration
{
    public static void ConfigureSwaggerServices(this IServiceCollection services)
    {
        services.AddSwaggerGen(option =>
        {

            option.MapType<DateOnly>(() => new OpenApiSchema { Type = "string", Format = "date" });

            option.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,
                $"{typeof(ServicesComposition).Assembly.GetName().Name}.xml"));

            option.OperationFilter<BadRequestResponseFilter>();
            option.OperationFilter<NotFoundResponseFilter>();
            option.OperationFilter<InternalServerErrorResponseFilter>();

            option.ExampleFilters();
        });

        services.AddSwaggerExamplesFromAssemblyOf(typeof(ServicesComposition));
    }
}
