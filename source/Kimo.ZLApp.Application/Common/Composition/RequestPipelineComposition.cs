using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Kimo.ZLApp.Application.Common.RequestPipelines;
using Kimo.ZLApp.Application.Common.RequestPipelines.Behaviors;

namespace Kimo.ZLApp.Application.Common.Composition;

[ExcludeFromCodeCoverage]
internal static class RequestPipelineComposition
{
    public static IServiceCollection ConfigureRequestPipelines(this IServiceCollection services)
    {
        return services.AddTransient(typeof(IPipelineBehavior<,>), typeof(HandleExceptionBehavior<,>))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>))
            .AddHandlersFromAssembly(typeof(AppComposition).Assembly)
            .AddScoped<IRequestExecutor, RequestExecutor>();
    }

    private static IServiceCollection AddHandlersFromAssembly(this IServiceCollection services, Assembly assembly)
    {
        var handlerTypes = assembly.GetTypes()
            .Where(t => t is { IsAbstract: false, IsInterface: false })
            .Where(t =>
                t.GetInterfaces()
                    .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)));

        foreach (var handlerType in handlerTypes)
        {
            var interfaces = handlerType.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>));

            foreach (var @interface in interfaces)
            {
                services.AddTransient(@interface, handlerType);
            }
        }

        return services;
    }
}