using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Kimo.ZLApp.Application.Common.Results;
using Kimo.ZLApp.Application.Common.Results.Error;
using Kimo.ZLApp.Application.Common.Results.ResultModels;
using Kimo.ZLApp.Application.Locations.Models;
using Kimo.ZLApp.Web.Common.Results;

namespace Kimo.ZLApp.Web.UnitTests.Common.Results;

public class ResultExtensionsTests
{
    [Fact]
    public void ToActionResult_ShouldReturnNoContent_WhenSuccessIsDeletedResult()
    {
        var result = new DeletedResult(10, 8, 2);

        var actionResult = result.ToActionResult();

        actionResult.ShouldBeOfType<NoContentResult>();
    }

    [Fact]
    public void ToActionResult_ShouldReturnProblemDetails_WhenFailure()
    {
        var error = new Error(ErrorType.NotFound, "Resource not found");
        var result = new Result(error);

        var actionResult = result.ToActionResult();

        var objectResult = actionResult.ShouldBeOfType<ObjectResult>();
        objectResult.StatusCode.ShouldBe(StatusCodes.Status404NotFound);

        var pd = objectResult.Value.ShouldBeOfType<ProblemDetails>();
        pd.Title.ShouldBe("Not Found");
        pd.Type.ShouldBe("https://kimo-online.de/developers/docs/problems/notfound");
        pd.Detail.ShouldBe("Resource not found");
        pd.Status.ShouldBe(StatusCodes.Status404NotFound);
    }

    [Fact]
    public void ToActionResult_ShouldIncludeMetadata()
    {
        var error = Error.Validation([new ValidationResult("error", ["field"])]);
        var result = new Result(error);

        var actionResult = result.ToActionResult();

        var obj = actionResult.ShouldBeOfType<ObjectResult>();
        var pd = obj.Value.ShouldBeOfType<ProblemDetails>();

        pd.Extensions.ShouldContainKey("validationErrors");

        var errors = pd.Extensions["validationErrors"].ShouldBeOfType<Dictionary<string, object>>();
        errors.ShouldContainKey("field");
        errors["field"].ShouldBe("error");
    }

    [Fact]
    public void ToActionResultT_ShouldReturnCreatedAtRoute_ForCreatedResult()
    {
        var result = new CreatedResult<LocationDto>(
            "/location/123",
            new LocationDto(123, "Test")
        );

        var actionResult = result.ToActionResult();

        var created = actionResult.Result.ShouldBeOfType<CreatedResult>();
        created.Location.ShouldBe("/location/123");
        created.Value.ShouldBeOfType<LocationDto>();
    }

    [Fact]
    public void ToActionResultT_ShouldReturnOkObjectResult_WhenValueExists()
    {
        var result = new Result<string>("value");

        var actionResult = result.ToActionResult();

        var ok = actionResult.Result.ShouldBeOfType<OkObjectResult>();
        ok.Value.ShouldBe("value");
    }

    [Fact]
    public void ToActionResultT_ShouldReturnNoContent_WhenValueNull()
    {
        var result = new Result<string?>((string?)null);

        var actionResult = result.ToActionResult();

        actionResult.Result.ShouldBeOfType<NoContentResult>();
    }

    [Fact]
    public void ToActionResultT_ShouldReturnProblemDetails_WhenFailure()
    {
        var error = new Error(ErrorType.Forbidden, "Forbidden");
        var result = new Result<string>(error);

        var actionResult = result.ToActionResult();

        var obj = actionResult.Result.ShouldBeOfType<ObjectResult>();
        var pd = obj.Value.ShouldBeOfType<ProblemDetails>();

        pd.Title.ShouldBe("Forbidden");
        pd.Status.ShouldBe(StatusCodes.Status403Forbidden);
        pd.Detail.ShouldBe("Forbidden");
    }

    [Fact]
    public async Task ToActionResultT_Task_ShouldReturnOkObject()
    {
        var result = new Result<int>(42);
        var task = Task.FromResult(result);

        var actionResult = await task.ToActionResult();

        var ok = actionResult.Result.ShouldBeOfType<OkObjectResult>();
        ok.Value.ShouldBe(42);
    }

    [Theory]
    [InlineData(ErrorType.Validation, "Validation error")]
    [InlineData(ErrorType.Conflict, "Conflict")]
    [InlineData(ErrorType.NotFound, "Not Found")]
    [InlineData(ErrorType.Unauthorized, "Unauthorized")]
    [InlineData(ErrorType.Forbidden, "Forbidden")]
    [InlineData(ErrorType.DependencyNotFound, "Dependency not found")]
    [InlineData(ErrorType.Unexpected, "Server error")]
    public void ToActionResult_ShouldMapTitles(ErrorType type, string expectedTitle)
    {
        // Arrange
        var result = new Result(new Error(type, "x"));

        // Act
        var action = result.ToActionResult();

        // Assert
        var obj = action.ShouldBeOfType<ObjectResult>();
        var details = obj.Value.ShouldBeOfType<ProblemDetails>();

        details.Title.ShouldBe(expectedTitle);
    }
}
