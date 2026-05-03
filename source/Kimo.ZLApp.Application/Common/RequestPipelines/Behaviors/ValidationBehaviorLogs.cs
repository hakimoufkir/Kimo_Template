using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace Kimo.ZLApp.Application.Common.RequestPipelines.Behaviors;

[ExcludeFromCodeCoverage]
public static partial class ValidationBehaviorLogs
{
    [LoggerMessage(
        EventId = ApplicationLogEventIds.Pipelines.Behaviors.ValidationBehavior.NoValidatorsFound,
        Level = LogLevel.Debug,
        Message = "No validators found for request of type {RequestType}.")]
    public static partial void NoValidatorsFound(this ILogger logger, string requestType);

    [LoggerMessage(
        EventId = ApplicationLogEventIds.Pipelines.Behaviors.ValidationBehavior.ValidationFailed,
        Level = LogLevel.Information,
        Message = "Validation failed for request of type {RequestType}. It has {FailuresCount} failures.")]
    public static partial void ValidationFailed(this ILogger logger, string requestType, int failuresCount);

    [LoggerMessage(
        EventId = ApplicationLogEventIds.Pipelines.Behaviors.ValidationBehavior.ValidationPassed,
        Level = LogLevel.Debug,
        Message = "Validation passed for request of type {RequestType}.")]
    public static partial void ValidationPassed(this ILogger logger, string requestType);
}