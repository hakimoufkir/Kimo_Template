using Microsoft.Extensions.Logging;
using Kimo.ZLApp.Application.Common.RequestPipelines;
using Kimo.ZLApp.Application.Common.RequestPipelines.Behaviors;
using Kimo.ZLApp.Application.Common.Results;

namespace Kimo.ZLApp.Application.UnitTests.Common.RequestPipelines.Behaviors;

public class HandleExceptionBehaviorTests
{
    [Fact]
    public async Task HandleAsync_CallsNext_WhenNoException()
    {
        // Arrange
        var logger = Substitute.For<ILogger<HandleExceptionBehavior<TestRequest, Result<string>>>>();
        var behavior = new HandleExceptionBehavior<TestRequest, Result<string>>(logger);

        var next = Substitute.For<RequestHandlerDelegate<Result<string>>>();
        var expectedResult = new Result<string>("Success");
        next.Invoke().Returns(Task.FromResult(expectedResult));

        var request = new TestRequest { Name = "Test" };

        // Act
        var result = await behavior.HandleAsync(request, next, CancellationToken.None);

        // Assert
        result.ShouldBe(expectedResult);
    }

    [Fact]
    public async Task HandleAsync_ReturnsFailedResult_OnException()
    {
        // Arrange
        var logger = Substitute.For<ILogger<HandleExceptionBehavior<TestRequest, Result<string>>>>();
        var behavior = new HandleExceptionBehavior<TestRequest, Result<string>>(logger);

        var next = Substitute.For<RequestHandlerDelegate<Result<string>>>();
        var expectedException = new InvalidOperationException("Error");
        next.Invoke().Returns<Task<Result<string>>>(_ => throw expectedException);

        var request = new TestRequest { Name = "Test" };

        // Act
        var result = await behavior.HandleAsync(request, next, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeFalse();
        result.Error.Message.ShouldContain("Error");
    }
}
