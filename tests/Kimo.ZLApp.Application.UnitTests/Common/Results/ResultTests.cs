using Kimo.ZLApp.Application.Common.Results;
using Kimo.ZLApp.Application.Common.Results.Error;

namespace Kimo.ZLApp.Application.UnitTests.Common.Results;

public class ResultImplicitOperatorTests
{
    [Fact]
    public void ImplicitOperator_FromValue_ShouldCreateSuccessResult()
    {
        // Arrange
        var value = 42;

        // Act
        Result<int> result = value;

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.IsError.ShouldBeFalse();
        result.Value.ShouldBe(value);
    }

    [Fact]
    public void ImplicitOperator_FromError_ShouldCreateErrorResult()
    {
        // Arrange
        var error = new Error(ErrorType.Failure, "Some error");

        // Act
        Result<int> result = error;

        // Assert
        result.IsError.ShouldBeTrue();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBe(error);
    }

    [Fact]
    public void ResultNonGenericImplicitOperator_FromError_ShouldCreateErrorResult()
    {
        // Arrange
        var error = new Error(ErrorType.Forbidden, "Some error");

        // Act
        Result result = error;

        // Assert
        result.IsError.ShouldBeTrue();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBe(error);
    }
}
