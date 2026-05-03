using System.Diagnostics.CodeAnalysis;

namespace Kimo.ZLApp.Web.Common.Composition;

[ExcludeFromCodeCoverage]
internal static class CorsComposition
{
    private const string CorsPolicy = "_allowedOrigins";

    public static IServiceCollection ConfigureCors(this IServiceCollection services)
    {
        return services.AddCors(options =>
        {
            options.AddPolicy(CorsPolicy, policyBuilder =>
            {
                policyBuilder.WithOrigins("https://localhost:5002")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });
    }

    public static IApplicationBuilder ConfigureCors(this IApplicationBuilder application)
    {
        return application.UseCors(CorsPolicy);
    }
}