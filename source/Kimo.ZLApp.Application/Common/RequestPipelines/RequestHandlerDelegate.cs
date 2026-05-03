namespace Kimo.ZLApp.Application.Common.RequestPipelines;

public delegate Task<TResponse> RequestHandlerDelegate<TResponse>();