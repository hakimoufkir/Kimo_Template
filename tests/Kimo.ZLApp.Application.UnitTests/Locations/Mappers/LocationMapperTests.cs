using Kimo.ZLApp.Application.Locations.Mappers;
using Kimo.ZLApp.Domain.Locations;
using Kimo.ZLApp.Tests.Common.Locations;

namespace Kimo.ZLApp.Application.UnitTests.Locations.Mappers;

public class LocationMapperTests
{
    [Fact]
    public void ToDto_ShouldMapSingleLocationCorrectly()
    {
        // Arrange
        var location = TestLocation.Default;

        // Act
        var result = location.ToDto();

        // Assert
        result.ShouldNotBeNull();
        result.Id.ShouldBe(location.Id);
        result.Name.ShouldBe(location.Name);
    }

    [Fact]
    public void ToDto_ShouldMapEnumerableOfLocationsCorrectly()
    {
        // Arrange
        var locations = new List<Location> { TestLocation.Default, TestLocation.Alternative };

        // Act
        var result = locations.ToDto();

        // Assert
        result.ShouldNotBeNull();
        result.Count.ShouldBe(2);

        result[0].ShouldBeEquivalentTo(TestLocation.Default.ToDto());
        result[1].ShouldBeEquivalentTo(TestLocation.Alternative.ToDto());
    }

    [Fact]
    public void ToDto_ShouldReturnEmptyList_WhenEnumerableIsEmpty()
    {
        // Arrange
        var locations = Enumerable.Empty<Location>();

        // Act
        var result = locations.ToDto();

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeEmpty();
    }
}
