using Microsoft.Extensions.Logging;
using Kimo.ZLApp.Application.Common.RequestPipelines;
using Kimo.ZLApp.Application.Common.RequestPipelines.Behaviors;
using Kimo.ZLApp.Application.Common.Results;
using Kimo.ZLApp.Application.Common.Results.Error;

namespace Kimo.ZLApp.Application.UnitTests.Common.RequestPipelines.Behaviors;

public class ValidationBehaviorTests
{
    [Fact]
    public async Task HandleAsync_ReturnsFailedResult_WhenValidationFails()
    {
        // Arrange
        var logger = Substitute.For<ILogger<ValidationBehavior<TestRequest, Result<string>>>>();
        var serviceProvider = Substitute.For<IServiceProvider>();
        var behavior = new ValidationBehavior<TestRequest, Result<string>>(logger, serviceProvider);

        var request = new TestRequest { Name = "Too Short" };

        var next = Substitute.For<RequestHandlerDelegate<Result<string>>>();

        // Act
        var result = await behavior.HandleAsync(request, next, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeFalse();
        result.Error.Type.ShouldBe(ErrorType.Validation);
    }

    [Fact]
    public async Task HandleAsync_CallsNext_WhenValidationPasses()
    {
        // Arrange
        var logger = Substitute.For<ILogger<ValidationBehavior<TestRequest, Result<string>>>>();
        var serviceProvider = Substitute.For<IServiceProvider>();
        var behavior = new ValidationBehavior<TestRequest, Result<string>>(logger, serviceProvider);

        var request = new TestRequest { Name = "Long Enough" };
        var next = Substitute.For<RequestHandlerDelegate<Result<string>>>();
        var expectedResult = new Result<string>("Success");
        next.Invoke().Returns(Task.FromResult(expectedResult));

        // Act
        var result = await behavior.HandleAsync(request, next, CancellationToken.None);

        // Assert
        result.ShouldBe(expectedResult);
    }
}
