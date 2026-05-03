using System.Diagnostics.CodeAnalysis;
using Hangfire.Server;
using Serilog.Context;

namespace Kimo.ZLApp.WorkerService.Common.Logging;

[ExcludeFromCodeCoverage]
public class HangfireLogContextJobPerformer(BackgroundJobPerformer innerPerformer) : IBackgroundJobPerformer
{
    public object Perform(PerformContext context)
    {
        using (LogContext.PushProperty("JobId", context.BackgroundJob.Id))
        using (LogContext.PushProperty("JobName", context.BackgroundJob.Job.Type.Name))
        {
            return innerPerformer.Perform(context);
        }
    }
}