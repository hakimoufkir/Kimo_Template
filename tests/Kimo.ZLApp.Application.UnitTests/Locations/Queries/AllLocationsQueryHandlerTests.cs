using Microsoft.Extensions.Logging;
using Kimo.ZLApp.Application.Locations;
using Kimo.ZLApp.Application.Locations.Mappers;
using Kimo.ZLApp.Application.Locations.Queries;
using Kimo.ZLApp.Domain.Locations;
using Kimo.ZLApp.Tests.Common.Locations;

namespace Kimo.ZLApp.Application.UnitTests.Locations.Queries;

public class AllLocationsQueryHandlerTests
{
    private readonly ILocationRepository _locationRepository = Substitute.For<ILocationRepository>();
    private readonly ILogger<AllLocationsQueryHandler> _logger = Substitute.For<ILogger<AllLocationsQueryHandler>>();

    private AllLocationsQueryHandler CreateHandler()
    {
        return new AllLocationsQueryHandler(_locationRepository, _logger);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnAllLocations_AsDto()
    {
        // Arrange
        var locations = new List<Location> { TestLocation.Default, TestLocation.Alternative };

        _locationRepository.GetAllAsync().Returns(locations);

        var queryHandler = CreateHandler();
        var query = new AllLocationsQuery();

        // Act
        var result = await queryHandler.HandleAsync(query);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeTrue();

        var data = result.Value;
        data.ShouldNotBeNull();
        data.Count.ShouldBe(2);
        data[0].ShouldBeEquivalentTo(TestLocation.Default.ToDto());
        data[1].ShouldBeEquivalentTo(TestLocation.Alternative.ToDto());
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnEmptyList_WhenNoLocationsExist()
    {
        // Arrange
        _locationRepository.GetAllAsync().Returns(new List<Location>());

        var sut = CreateHandler();
        var query = new AllLocationsQuery();

        // Act
        var result = await sut.HandleAsync(query);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBeEmpty();
    }
}
