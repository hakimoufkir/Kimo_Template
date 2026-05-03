using Microsoft.Extensions.DependencyInjection;
using Kimo.ZLApp.Application.Common.RequestPipelines;
using Kimo.ZLApp.Application.Common.Results;

namespace Kimo.ZLApp.Application.UnitTests.Common.RequestPipelines;

public class RequestExecutorTests
{
    [Fact]
    public async Task ExecuteAsync_HappyPath_ReturnsHandlerResult()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddTransient<IRequestHandler<TestRequest, Result<string>>, TestHandler>();
        services.AddTransient<IPipelineBehavior<TestRequest, Result<string>>, TestBehavior>();
        var provider = services.BuildServiceProvider();

        var executor = new RequestExecutor(provider);
        var request = new TestRequest { Name = "Test" };

        // Act
        var result = await executor.ExecuteAsync(request, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe("Success");
    }
}
