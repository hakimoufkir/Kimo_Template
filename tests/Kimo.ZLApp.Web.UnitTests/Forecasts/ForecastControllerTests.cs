using Microsoft.AspNetCore.Mvc;
using Kimo.ZLApp.Application.Common.Filtering.Responses;
using Kimo.ZLApp.Application.Common.RequestPipelines;
using Kimo.ZLApp.Application.Common.Results;
using Kimo.ZLApp.Application.Common.Results.ResultModels;
using Kimo.ZLApp.Application.WeatherForecasts.Commands;
using Kimo.ZLApp.Application.WeatherForecasts.Mappers;
using Kimo.ZLApp.Application.WeatherForecasts.Models;
using Kimo.ZLApp.Tests.Common.Forecasts;
using Kimo.ZLApp.Tests.Common.Locations;
using Kimo.ZLApp.Web.Common.Filtering;
using Kimo.ZLApp.Web.Forecasts;

namespace Kimo.ZLApp.Web.UnitTests.Forecasts;

public class ForecastControllerTests
{
    private readonly IRequestExecutor _requestExecutor = Substitute.For<IRequestExecutor>();

    private ForecastController CreateController()
    {
        return new ForecastController(_requestExecutor);
    }

    [Fact]
    public async Task GetAll_ShouldReturnResultFromRequestExecutor()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;

        var forecasts = new List<ForecastDto> { TestForecast.Default(TestLocation.Default).ToDto() };

        _requestExecutor
            .ExecuteAsync(Arg.Any<IRequest<Result<PaginatedData<ForecastDto>>>>(), cancellationToken)
            .Returns(Task.FromResult(
                new Result<PaginatedData<ForecastDto>>(
                    new PaginatedData<ForecastDto>(forecasts, 1, 10, 1))));

        var controller = CreateController();

        // Act
        var result = await controller.Get(new QueryParameters { Skip = 0, Take = 10, Sort = "asc", Order = "id" },
            cancellationToken);

        // Assert
        var okResult = result.Result as OkObjectResult;
        okResult.ShouldNotBeNull();
        var data = okResult.Value as PaginatedData<ForecastDto>;
        data.ShouldNotBeNull();
        data.Data.ShouldBe(forecasts);
        data.Page.ShouldBe(1);
        data.PageSize.ShouldBe(10);
        data.TotalRecords.ShouldBe(1);
    }

    [Fact]
    public async Task GetSingle_ShouldReturnResultFromRequestExecutor()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var forecast = TestForecast.Default(TestLocation.Default).ToDto();

        _requestExecutor
            .ExecuteAsync(Arg.Any<IRequest<Result<ForecastDto>>>(), cancellationToken)
            .Returns(Task.FromResult(new Result<ForecastDto>(forecast)));

        var controller = CreateController();

        // Act
        var result = await controller.Get(1, cancellationToken);

        // Assert
        var okResult = result.Result as OkObjectResult;
        okResult.ShouldNotBeNull();
        okResult.Value.ShouldBe(forecast);
    }

    [Fact]
    public async Task Delete_ShouldCallExecutorWithCommand()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var ids = new List<int> { 1, 2 };

        _requestExecutor
            .ExecuteAsync(Arg.Any<IRequest<Result>>(), cancellationToken)
            .Returns(Task.FromResult(Result.Success()));

        var controller = CreateController();

        // Act
        var result = await controller.Delete(ids, cancellationToken);

        // Assert
        var noContentResult = result as NoContentResult;
        noContentResult.ShouldNotBeNull();

        await _requestExecutor.Received(1)
            .ExecuteAsync(Arg.Is<DeleteForecastsCommand>(c => c.ForecastIds == ids), cancellationToken);
    }

    [Fact]
    public async Task Post_ShouldReturnResultFromRequestExecutor()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var command = new CreateForecastCommand
        {
            Date = new DateOnly(2026, 1, 5),
            LocationId = 1,
            Hour = 20,
            Temperature = 30.5
        };

        var createdForecast = TestForecast.Alternative(TestLocation.Default);
        createdForecast.DefineTemperatureForecast(20, 30.5);
        var createdForecastResult = createdForecast.ToDto();

        _requestExecutor
            .ExecuteAsync(Arg.Any<IRequest<Result<ForecastDto>>>(), cancellationToken)
            .Returns(Task.FromResult<Result<ForecastDto>>(
                new CreatedResult<ForecastDto>("forecasts/1", createdForecastResult)));

        var controller = CreateController();

        // Act
        var result = await controller.Post(command, cancellationToken);

        // Assert
        var createdResult = result.Result as CreatedResult;
        createdResult.ShouldNotBeNull();
        createdResult.Value.ShouldBe(createdForecastResult);
    }
}
