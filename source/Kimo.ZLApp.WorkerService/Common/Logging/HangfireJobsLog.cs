using System.Diagnostics.CodeAnalysis;

namespace Kimo.ZLApp.WorkerService.Common.Logging;

[ExcludeFromCodeCoverage]
public static partial class HangfireJobsLog
{
    [LoggerMessage(
        EventId = WorkerServiceLogEventId.HangfireJobCreating,
        Level = LogLevel.Information,
        Message = "Creating Hangfire job {TypeName} for method {MethodName} with arguments {JobArguments}.")]
    public static partial void HangfireJobCreating(
        this ILogger<HangfireJobLoggerAttribute> logger,
        string typeName,
        string methodName,
        IReadOnlyCollection<object> jobArguments
    );

    [LoggerMessage(
        EventId = WorkerServiceLogEventId.HangfireJobCreated,
        Level = LogLevel.Information,
        Message = "Hangfire job {JobId} created.")]
    public static partial void HangfireJobCreated(this ILogger<HangfireJobLoggerAttribute> logger, string jobId);

    [LoggerMessage(
        EventId = WorkerServiceLogEventId.HangfireJobPerforming,
        Level = LogLevel.Information,
        Message = "Performing Hangfire job {JobId}.")]
    public static partial void HangfireJobPerforming(this ILogger<HangfireJobLoggerAttribute> logger, string jobId);

    [LoggerMessage(
        EventId = WorkerServiceLogEventId.HangfireJobPerformed,
        Level = LogLevel.Information,
        Message = "Hangfire job {JobId} performed.")]
    public static partial void HangfireJobPerformed(this ILogger<HangfireJobLoggerAttribute> logger, string jobId);

    [LoggerMessage(
        EventId = WorkerServiceLogEventId.HangfireJobFailed,
        Level = LogLevel.Error,
        Message = "Hangfire job {JobId} failed.")]
    public static partial void HangfireJobFailed(
        this ILogger<HangfireJobLoggerAttribute> logger,
        Exception exception,
        string jobId
    );

    [LoggerMessage(
        EventId = WorkerServiceLogEventId.HangfireJobStateElection,
        Level = LogLevel.Information,
        Message = "Hangfire job {JobId} state election from state {FromState} to state {ToState}.")]
    public static partial void HangfireJobStateElection(
        this ILogger<HangfireJobLoggerAttribute> logger,
        string jobId,
        string fromState,
        string toState
    );

    [LoggerMessage(
        EventId = WorkerServiceLogEventId.HangfireJobStateApplied,
        Level = LogLevel.Information,
        Message = "Hangfire job {JobId} applied state {State}.")]
    public static partial void HangfireJobStateApplied(
        this ILogger<HangfireJobLoggerAttribute> logger,
        string jobId,
        string state
    );

    [LoggerMessage(
        EventId = WorkerServiceLogEventId.HangfireJobStateUnapplied,
        Level = LogLevel.Information,
        Message = "Hangfire job {JobId} unapplied state {State}.")]
    public static partial void HangfireJobStateUnapplied(
        this ILogger<HangfireJobLoggerAttribute> logger,
        string jobId,
        string state
    );
}