using Kimo.ZLApp.Application.Common.RequestPipelines;
using Kimo.ZLApp.Application.Common.Results;

namespace Kimo.ZLApp.Application.UnitTests.Common.RequestPipelines;

public class TestHandler : IRequestHandler<TestRequest, Result<string>>
{
    public Task<Result<string>> HandleAsync(TestRequest request, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new Result<string>("Success"));
    }
}
