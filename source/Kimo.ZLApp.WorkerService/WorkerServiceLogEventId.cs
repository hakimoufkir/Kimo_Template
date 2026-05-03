namespace Kimo.ZLApp.WorkerService;

public static class WorkerServiceLogEventId // 8000-8999
{
    // Hangfire Jobs 8200-8299
    public const int HangfireJobCreating = 8200;
    public const int HangfireJobCreated = 8201;
    public const int HangfireJobPerforming = 8202;
    public const int HangfireJobPerformed = 8203;
    public const int HangfireJobFailed = 8204;

    // Job States 8230-8239
    public const int HangfireJobStateElection = 8230;
    public const int HangfireJobStateApplied = 8231;
    public const int HangfireJobStateUnapplied = 8232;
}