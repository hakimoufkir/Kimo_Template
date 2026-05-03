using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace Kimo.ZLApp.Application.Common.RequestPipelines.Behaviors;

[ExcludeFromCodeCoverage]
public static partial class HandleExceptionBehaviorLogs
{
    [LoggerMessage(
        EventId = ApplicationLogEventIds.Pipelines.Behaviors.HandleExceptionBehavior
            .UnhandledExceptionWhileProcessingRequest,
        Level = LogLevel.Error,
        Message = "An unhandled exception occurred while processing the request.")]
    public static partial void UnhandledExceptionWhileProcessingRequest(this ILogger logger, Exception exception);
}