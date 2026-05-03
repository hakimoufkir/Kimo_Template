using System.Diagnostics.CodeAnalysis;
using Hangfire.Client;
using Hangfire.Common;
using Hangfire.Server;
using Hangfire.States;
using Hangfire.Storage;

namespace Kimo.ZLApp.WorkerService.Common.Logging;

[ExcludeFromCodeCoverage]
public class HangfireJobLoggerAttribute(ILogger<HangfireJobLoggerAttribute> logger)
    : JobFilterAttribute, IClientFilter, IServerFilter, IElectStateFilter, IApplyStateFilter
{
    public void OnStateApplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
    {
        logger.HangfireJobStateApplied(context.BackgroundJob.Id, context.NewState.Name);
    }

    public void OnStateUnapplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
    {
        logger.HangfireJobStateUnapplied(context.BackgroundJob.Id, context.OldStateName);
    }

    public void OnCreating(CreatingContext context)
    {
        logger.HangfireJobCreating(context.Job.Type.Name, context.Job.Method.Name, context.Job.Args);
    }

    public void OnCreated(CreatedContext context)
    {
        logger.HangfireJobCreated(context.BackgroundJob.Id);
    }

    public void OnStateElection(ElectStateContext context)
    {
        logger.HangfireJobStateElection(context.BackgroundJob.Id,
            context.CurrentState, context.CandidateState.Name);
    }

    public void OnPerforming(PerformingContext context)
    {
        logger.HangfireJobPerforming(context.BackgroundJob.Id);
    }

    public void OnPerformed(PerformedContext context)
    {
        logger.HangfireJobPerformed(context.BackgroundJob.Id);

        if (context.Exception is not null)
        {
            logger.HangfireJobFailed(context.Exception, context.BackgroundJob.Id);
        }
    }
}