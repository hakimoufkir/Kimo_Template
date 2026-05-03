using Kimo.ZLApp.Application.Common.Results;
using Kimo.ZLApp.Application.Common.Results.Error;
using Kimo.ZLApp.Application.Common.Results.ResultModels;

namespace Kimo.ZLApp.Web.Common.Results;

/// <summary>
///     Result Extensions.
/// </summary>
public static class ResultExtensions
{
    /// <summary>
    ///     Maps a Result to an ActionResult.
    /// </summary>
    /// <param name="result">The Result that should be mapped.</param>
    /// <returns>The mapped ActionResult.</returns>
    public static ActionResult ToActionResult(this Result result)
    {
        if (result.IsSuccess)
        {
            return result switch
            {
                DeletedResult => new NoContentResult(),
                _ => new NoContentResult()
            };
        }

        return result.Error.ToActionResult();
    }

    /// <summary>
    ///     Maps a Result to an ActionResult.
    /// </summary>
    /// <param name="result">The Result that should be mapped.</param>
    /// <typeparam name="T">The expected datatype of the result.</typeparam>
    /// <returns>The mapped ActionResult.</returns>
    public static ActionResult<T> ToActionResult<T>(this Result<T> result)
    {
        if (result.IsSuccess)
        {
            return result switch
            {
                CreatedResult<T> createdResult => new CreatedResult(createdResult.Location, createdResult.Value),
                null => new NoContentResult(),
                _ when result.Value is not null => new OkObjectResult(result.Value),
                _ => new NoContentResult()
            };
        }

        return result.Error.ToActionResult<T>();
    }

    /// <summary>
    ///     Maps a Result to an ActionResult.
    /// </summary>
    /// <param name="resultTask">The task that contains the Result that should be mapped.</param>
    /// <typeparam name="T">The expected datatype of the result.</typeparam>
    /// <returns>The mapped ActionResult.</returns>
    public static async Task<ActionResult<T>> ToActionResult<T>(this Task<Result<T>> resultTask)
    {
        var result = await resultTask;
        return result.ToActionResult();
    }

    private static ActionResult<T> ToActionResult<T>(this Error error)
    {
        return error.ToActionResult();
    }

    private static ActionResult ToActionResult(this Error error)
    {
        var details = new ProblemDetails
        {
            Type = $"https://kimo-online.de/developers/docs/problems/{error.Type.ToString().ToLowerInvariant()}",
            Title = error.Type.GetTitle(),
            Status = error.Type.GetStatusCode(),
            Detail = error.Message
        };

        if (error.Metadata is not { Count: > 0 })
        {
            return new ObjectResult(details) { StatusCode = details.Status };
        }

        foreach (var kvp in error.Metadata)
        {
            details.Extensions[kvp.Key] = kvp.Value;
        }

        return new ObjectResult(details) { StatusCode = details.Status };
    }

    private static string GetTitle(this ErrorType type)
    {
        return type switch
        {
            ErrorType.Validation => "Validation error",
            ErrorType.Conflict => "Conflict",
            ErrorType.NotFound => "Not Found",
            ErrorType.Unauthorized => "Unauthorized",
            ErrorType.Forbidden => "Forbidden",
            ErrorType.DependencyNotFound => "Dependency not found",
            ErrorType.Unexpected => "Server error",
            _ => "Request error"
        };
    }

    private static int GetStatusCode(this ErrorType type)
    {
        return type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            ErrorType.DependencyNotFound => StatusCodes.Status400BadRequest,
            ErrorType.Unexpected => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status400BadRequest
        };
    }
}