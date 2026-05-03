using Kimo.ZLApp.Application.Common.RequestPipelines;
using Kimo.ZLApp.Application.Common.Results;

namespace Kimo.ZLApp.Application.UnitTests.Common.RequestPipelines;

public class TestBehavior : IPipelineBehavior<TestRequest, Result<string>>
{
    public Task<Result<string>> HandleAsync(TestRequest request, RequestHandlerDelegate<Result<string>> next,
        CancellationToken cancellationToken)
    {
        return next();
    }
}
