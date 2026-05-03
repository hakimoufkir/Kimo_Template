namespace Kimo.ZLApp.Application.Common.RequestPipelines;

public interface IPipelineBehavior<in TRequest, TResponse> where TRequest : notnull
{
    Task<TResponse> HandleAsync(TRequest request, RequestHandlerDelegate<TResponse> nextBehavior,
        CancellationToken cancellationToken);
}