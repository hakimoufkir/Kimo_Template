using Kimo.ZLApp.Application.Common.RequestPipelines.Behaviors;
using Kimo.ZLApp.Application.Common.Results;
using Kimo.ZLApp.Application.Common.Results.Error;

namespace Kimo.ZLApp.Application.UnitTests.Common.RequestPipelines.Behaviors;

public class BehaviorResultTests
{
    [Fact]
    public void Fail_CreatesGenericResult_WithError()
    {
        // Arrange
        var error = Error.Unexpected(new Exception("test"));

        // Act
        var result = BehaviorResult.Fail<Result<string>>(error);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBe(error);
    }

    [Fact]
    public void Fail_CreatesNonGenericResult_WithError()
    {
        // Arrange
        var error = Error.Unexpected(new Exception("test"));

        // Act
        var result = BehaviorResult.Fail<Result>(error);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBe(error);
    }

    [Fact]
    public void Fail_Throws_WhenResponseTypeIsInvalid()
    {
        // Arrange
        var error = Error.Unexpected(new Exception("test"));

        // Act & Assert
        Should.Throw<InvalidOperationException>(() =>
            BehaviorResult.Fail<string>(error)
        ).Message.ShouldContain("does not implement Result or Result<T>");
    }

    [Fact]
    public void IsResult_ShouldBeTrue_ForResult()
    {
        // Act
        var isResult = BehaviorResult.IsResult<Result>();

        // Assert
        isResult.ShouldBeTrue();
    }

    [Fact]
    public void IsResult_ShouldBeTrue_ForGenericResult()
    {
        // Act
        var isResult = BehaviorResult.IsResult<Result<string>>();

        // Assert
        isResult.ShouldBeTrue();
    }

    [Fact]
    public void IsResult_ShouldBeFalse_ForNonResultTypes()
    {
        // Act
        var isResult = BehaviorResult.IsResult<int>();

        // Assert
        isResult.ShouldBeFalse();
    }
}
