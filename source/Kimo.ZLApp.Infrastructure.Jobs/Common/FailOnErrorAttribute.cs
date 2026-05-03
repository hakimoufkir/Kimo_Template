using Hangfire.Common;
using Hangfire.Server;
using Kimo.ZLApp.Application.Common.Results;
using Kimo.ZLApp.Application.Common.Results.Error;

namespace Kimo.ZLApp.Infrastructure.Jobs.Common;

[AttributeUsage(AttributeTargets.Class)]
public class FailOnErrorAttribute : JobFilterAttribute, IServerFilter
{
    private readonly HashSet<ErrorType>? _errorTypes;

    public FailOnErrorAttribute()
    {
        _errorTypes = null;
    }

    public FailOnErrorAttribute(params ErrorType[] errorTypes)
    {
        _errorTypes = [.. errorTypes];
    }

    public void OnPerforming(PerformingContext context)
    {
    }

    public void OnPerformed(PerformedContext context)
    {
        if (context.Result is not Result result)
        {
            return;
        }

        if (result.IsSuccess)
        {
            return;
        }

        if (_errorTypes is null || _errorTypes.Contains(result.Error.Type))
        {
            throw new ApplicationException(result.Error.Metadata?["exception"].ToString() ??
                                           $"Job failed due to application error: {result.Error.Type} - {result.Error.Message}");
        }
    }
}
