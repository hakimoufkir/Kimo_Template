namespace Kimo.ZLApp.Application.Common.RequestPipelines;

public interface IRequestExecutor
{
    Task<TResponse> ExecuteAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken);
}