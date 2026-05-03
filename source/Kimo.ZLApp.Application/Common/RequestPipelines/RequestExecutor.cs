using Microsoft.Extensions.DependencyInjection;

namespace Kimo.ZLApp.Application.Common.RequestPipelines;

public class RequestExecutor(IServiceProvider serviceProvider) : IRequestExecutor
{
    public Task<TResponse> ExecuteAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken)
    {
        var requestType = request.GetType();
        var responseType = typeof(TResponse);

        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, responseType);
        var pipelineType = typeof(IPipelineBehavior<,>).MakeGenericType(requestType, responseType);

        var behaviors = serviceProvider.GetServices(pipelineType);
        var handler = serviceProvider.GetRequiredService(handlerType);

        RequestHandlerDelegate<TResponse> next = () =>
            ((dynamic)handler).HandleAsync((dynamic)request, cancellationToken);

        foreach (var behavior in behaviors.Cast<dynamic>().Reverse())
        {
            var currentNext = next;
            next = () => behavior.HandleAsync((dynamic)request, currentNext, cancellationToken);
        }

        return next();
    }
}