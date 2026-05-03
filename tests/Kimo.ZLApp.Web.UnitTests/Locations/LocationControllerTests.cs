using Microsoft.AspNetCore.Mvc;
using Kimo.ZLApp.Application.Common.RequestPipelines;
using Kimo.ZLApp.Application.Common.Results;
using Kimo.ZLApp.Application.Locations.Mappers;
using Kimo.ZLApp.Application.Locations.Models;
using Kimo.ZLApp.Tests.Common.Locations;
using Kimo.ZLApp.Web.Locations;

namespace Kimo.ZLApp.Web.UnitTests.Locations;

public class LocationControllerTests
{
    private readonly IRequestExecutor _requestExecutor = Substitute.For<IRequestExecutor>();

    private LocationController CreateController()
    {
        return new LocationController(_requestExecutor);
    }

    [Fact]
    public async Task Get_ShouldReturnResultFromRequestExecutor()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;

        var locations = new List<LocationDto> { TestLocation.Default.ToDto() };

        _requestExecutor
            .ExecuteAsync(Arg.Any<IRequest<Result<List<LocationDto>>>>(), cancellationToken)
            .Returns(Task.FromResult(new Result<List<LocationDto>>(locations)));


        var controller = CreateController();

        // Act
        var result = await controller.Get(cancellationToken);

        // Assert
        var okResult = result.Result as OkObjectResult;
        okResult.ShouldNotBeNull();
        okResult.Value.ShouldBe(locations);
    }
}
