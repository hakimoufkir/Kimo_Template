using Microsoft.Extensions.Logging;
using Kimo.ZLApp.Application.Common.Results.Error;

namespace Kimo.ZLApp.Application.Common.RequestPipelines.Behaviors;

public class HandleExceptionBehavior<TRequest, TResponse>(ILogger<HandleExceptionBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> HandleAsync(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            logger.UnhandledExceptionWhileProcessingRequest(ex);
            return BehaviorResult.Fail<TResponse>(Error.Unexpected(ex));
        }
    }
}