using Microsoft.AspNetCore.Mvc;
using Kimo.ZLApp.Application.Common.RequestPipelines;
using Kimo.ZLApp.Application.Common.Results;
using Kimo.ZLApp.Web.Common.RequestPipeline;

namespace Kimo.ZLApp.Web.UnitTests.Common.RequestPipeline;

public class RequestExecutorExtensionsTests
{
    [Fact]
    public async Task HandleForGenericResult_ShouldCallExecuteAsync_AndReturnActionResult()
    {
        // Arrange
        var executor = Substitute.For<IRequestExecutor>();
        var request = Substitute.For<IRequest<Result<string>>>();

        var expectedResult = new Result<string>("test value");
        executor.ExecuteAsync(request, Arg.Any<CancellationToken>()).Returns(Task.FromResult(expectedResult));

        // Act
        var actionResult = await executor.Handle(request);

        // Assert
        await executor.Received(1).ExecuteAsync(request, Arg.Any<CancellationToken>());
        var okResult = actionResult.Result.ShouldBeOfType<OkObjectResult>();
        okResult.Value.ShouldBe("test value");
    }

    [Fact]
    public async Task HandleForNonGenericResult_ShouldCallExecuteAsync_AndReturnActionResult()
    {
        // Arrange
        var executor = Substitute.For<IRequestExecutor>();
        var request = Substitute.For<IRequest<Result>>();

        var expectedResult = Result.Success();
        executor.ExecuteAsync(request, Arg.Any<CancellationToken>()).Returns(Task.FromResult(expectedResult));

        // Act
        var actionResult = await executor.Handle(request);

        // Assert
        await executor.Received(1).ExecuteAsync(request, Arg.Any<CancellationToken>());
        actionResult.ShouldBeOfType<NoContentResult>();
    }
}
