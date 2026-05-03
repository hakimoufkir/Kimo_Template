using Kimo.ZLApp.Domain.WeatherForecasts;
using Kimo.ZLApp.Tests.Common.Common;
using Kimo.ZLApp.Tests.Common.Locations;

namespace Kimo.ZLApp.Domain.UnitTests.WeatherForecasts;

public class ForecastTests
{
    private static readonly DateOnly s_currentDate = new(2026, 2, 1);

    [Fact]
    public void CtorAndId_OrmShouldBeAbleToInstantiate()
    {
        AggregateTesting.VerifyOrmCreationAndId<Forecast, int>(nameof(Forecast.Id), 25);
    }

    [Fact]
    public void Create_WithValidArguments_ShouldCreateInstance()
    {
        // Arrange
        var date = new DateOnly(2026, 2, 2);

        // Act
        var forecast = new Forecast(date, TestLocation.Default, s_currentDate);

        // Assert
        forecast.Id.ShouldBe(0);
        forecast.Date.ShouldBe(date);
        forecast.LocationId.ShouldBe(TestLocation.Default.Id);
        forecast.Location.ShouldBeEquivalentTo(TestLocation.Default);
    }

    [Fact]
    public void Create_WithInvalidLocation_ShouldThrow()
    {
        // Arrange
        var date = new DateOnly(2026, 2, 2);

        // Act
        var act = () => new Forecast(date, TestLocation.Invalid, s_currentDate);

        // Assert
        act.ShouldThrow<ArgumentException>();
    }

    [Fact]
    public void Create_WithDateInPast_ShouldThrow()
    {
        // Arrange
        var date = new DateOnly(2026, 1, 31);

        // Act
        var act = () => new Forecast(date, TestLocation.Default, s_currentDate);

        // Assert
        act.ShouldThrow<ArgumentException>();
    }

    [Fact]
    public void ChangeLocation_WithValidLocation_ShouldSetLocationToNewValue()
    {
        // Arrange
        var date = new DateOnly(2026, 2, 2);
        var forecast = new Forecast(date, TestLocation.Default, s_currentDate);

        // Act
        forecast.ChangeLocation(TestLocation.Alternative);

        // Assert
        forecast.Id.ShouldBe(0);
        forecast.LocationId.ShouldBe(TestLocation.Alternative.Id);
        forecast.Location.ShouldBeEquivalentTo(TestLocation.Alternative);
    }

    [Fact]
    public void ChangeLocation_WithInvalidLocation_ShouldThrow()
    {
        // Arrange
        var date = new DateOnly(2026, 2, 2);
        var forecast = new Forecast(date, TestLocation.Default, s_currentDate);

        // Act
        var act = () => forecast.ChangeLocation(TestLocation.Invalid);

        // Assert
        act.ShouldThrow<ArgumentException>();
    }
}
