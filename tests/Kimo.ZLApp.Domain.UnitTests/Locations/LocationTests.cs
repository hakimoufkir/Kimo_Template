using Kimo.ZLApp.Domain.Locations;
using Kimo.ZLApp.Tests.Common.Common;

namespace Kimo.ZLApp.Domain.UnitTests.Locations;

public class LocationTests
{
    [Fact]
    public void CtorAndId_OrmShouldBeAbleToInstantiate()
    {
        AggregateTesting.VerifyOrmCreationAndId<Location, int>(nameof(Location.Id), 25);
    }

    [Theory]
    [InlineData("Test")]
    [InlineData("  Spaces\nTest\t  ")]
    [InlineData("A")]
    [InlineData("This-Is-A-Super-Long-Name-With-One-Hundred-Characters-To-Check-If-The-Validation-Works-Fine-Or-Not-!")]
    public void Ctor_WithValidNameArguments_ShouldCreateInstance(string name)
    {
        // Arrange

        // Act
        var location = new Location(name);

        // Assert
        location.Id.ShouldBe(0);
        location.Name.ShouldBe(name);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")] // Space
    [InlineData("\n")] // Line Break
    [InlineData("\t")] // Tab
    [InlineData("   ")] // Multiple spaces
    [InlineData("\n\n\n")] // Multiple line breaks
    [InlineData("\t\t\t")] // Multiple tabs
    [InlineData("\t  \n\t  \n\t")] // Mixed
    public void Ctor_WithEmptyNameArguments_ShouldThrow(string name)
    {
        // Arrange

        // Act
        var act = () => new Location(name);

        // Assert
        act.ShouldThrow<ArgumentException>();
    }

    [Fact]
    public void Ctor_WithTooLongNameArguments_ShouldThrow()
    {
        // Arrange
        var name =
            "This-Is-A-Super-Long-Name-With-One-Hundred-And-One-Characters-To-Validate-If-The-Exception-Is-Thrown!";

        // Act
        var act = () => new Location(name);

        // Assert
        act.ShouldThrow<ArgumentOutOfRangeException>();
    }

    [Theory]
    [InlineData("Test")]
    [InlineData("  Spaces\nTest\t  ")]
    [InlineData("A")]
    [InlineData("This-Is-A-Super-Long-Name-With-One-Hundred-Characters-To-Check-If-The-Validation-Works-Fine-Or-Not-!")]
    public void Rename_WithValidNameArguments_ShouldRename(string newName)
    {
        // Arrange
        var location = new Location("Old Name");

        // Act
        location.Rename(newName);

        // Assert
        location.Name.ShouldBe(newName);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")] // Space
    [InlineData("\n")] // Line Break
    [InlineData("\t")] // Tab
    [InlineData("   ")] // Multiple spaces
    [InlineData("\n\n\n")] // Multiple line breaks
    [InlineData("\t\t\t")] // Multiple tabs
    [InlineData("\t  \n\t  \n\t")] // Mixed
    public void Rename_WithEmptyNameArguments_ShouldThrow(string newName)
    {
        // Arrange
        var location = new Location("Old Name");

        // Act
        var act = () => location.Rename(newName);

        // Assert
        act.ShouldThrow<ArgumentException>();
    }

    [Fact]
    public void Rename_WithTooLongNameArguments_ShouldThrow()
    {
        // Arrange
        var location = new Location("Old Name");
        var newName =
            "This-Is-A-Super-Long-Name-With-One-Hundred-And-One-Characters-To-Validate-If-The-Exception-Is-Thrown!";

        // Act
        var act = () => location.Rename(newName);

        // Assert
        act.ShouldThrow<ArgumentOutOfRangeException>();
    }
}
