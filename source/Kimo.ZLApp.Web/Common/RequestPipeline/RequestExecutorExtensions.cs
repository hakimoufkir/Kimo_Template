using Kimo.ZLApp.Application.Common.RequestPipelines;
using Kimo.ZLApp.Application.Common.Results;
using Kimo.ZLApp.Web.Common.Results;

namespace Kimo.ZLApp.Web.Common.RequestPipeline;

/// <summary>
///     Request pipeline extensions.
/// </summary>
public static class RequestExecutorExtensions
{
    /// <summary>
    ///     Handles the request and maps the result to an ActionResult.
    /// </summary>
    /// <param name="executor">The executing request pipeline.</param>
    /// <param name="request">The request itself.</param>
    /// <param name="cancellationToken">The CancellationToken which can be used to abort the request.</param>
    /// <typeparam name="TResult">The expected datatype of the result.</typeparam>
    /// <returns>The properly mapped ActionResult.</returns>
    public static async Task<ActionResult<TResult>> Handle<TResult>(
        this IRequestExecutor executor,
        IRequest<Result<TResult>> request,
        CancellationToken cancellationToken = default)
    {
        var result = await executor.ExecuteAsync(request, cancellationToken);
        return result.ToActionResult();
    }

    /// <summary>
    ///     Handles the request and maps the result to an ActionResult.
    /// </summary>
    /// <param name="executor">The executing request pipeline.</param>
    /// <param name="request">The request itself.</param>
    /// <param name="cancellationToken">The CancellationToken which can be used to abort the request.</param>
    /// <returns>The properly mapped ActionResult.</returns>
    public static async Task<ActionResult> Handle(
        this IRequestExecutor executor,
        IRequest<Result> request,
        CancellationToken cancellationToken = default)
    {
        var result = await executor.ExecuteAsync(request, cancellationToken);
        return result.ToActionResult();
    }
}