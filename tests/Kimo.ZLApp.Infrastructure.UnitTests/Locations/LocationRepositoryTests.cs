using Kimo.ZLApp.Infrastructure.Locations;
using Kimo.ZLApp.Infrastructure.UnitTests.Common;
using Kimo.ZLApp.Tests.Common.Locations;
using Shouldly;

namespace Kimo.ZLApp.Infrastructure.UnitTests.Locations;

public class LocationRepositoryTests
{
    private static async Task<TestDbContext> GenerateMockedContext()
    {
        var context = await TestDbContext.Create();

        context.Locations.AddRange(
            TestLocation.Default,
            TestLocation.Alternative
        );

        await context.SaveChangesAsync();
        return context;
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnLocationsOrderedByName()
    {
        // Arrange
        await using var context = await GenerateMockedContext();

        var repository = new LocationRepository(context);

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        result.Select(l => l.Name).ShouldBe([
            "Alternative Location",
            "Test Location"
        ]);
    }
}
