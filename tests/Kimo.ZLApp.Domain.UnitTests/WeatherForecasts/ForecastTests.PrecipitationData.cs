using Kimo.ZLApp.Tests.Common.Forecasts;
using Kimo.ZLApp.Tests.Common.Locations;

namespace Kimo.ZLApp.Domain.UnitTests.WeatherForecasts;

public class ForecastTestsPrecipitationData
{
    [Theory]
    [InlineData(10, 100)]
    [InlineData(0, 0)]
    [InlineData(23, 1000)]
    public void DefinePrecipitationForecast_WithValidData_ShouldAdd(int hour, int value)
    {
        // Arrange
        var forecast = TestForecast.Default(TestLocation.Default);

        // Act
        forecast.DefinePrecipitationForecast(hour, value);

        // Assert
        forecast.Precipitations.Count.ShouldBe(1);
        forecast.Precipitations.First().Hour.ShouldBe(hour);
        forecast.Precipitations.First().Value.ShouldBe(value);
    }

    [Theory]
    [InlineData(-1, 100)]
    [InlineData(24, 100)]
    public void DefinePrecipitationForecast_WithInvalidHours_ShouldThrow(int hour, int value)
    {
        // Arrange
        var forecast = TestForecast.Default(TestLocation.Default);

        // Act
        var act = () => forecast.DefinePrecipitationForecast(hour, value);

        // Assert
        act.ShouldThrow<ArgumentException>();
    }

    [Theory]
    [InlineData(5, -1)]
    [InlineData(5, 1001)]
    public void DefinePrecipitationForecast_WithInvalidValue_ShouldThrow(int hour, int value)
    {
        // Arrange
        var forecast = TestForecast.Default(TestLocation.Default);

        // Act
        var act = () => forecast.DefinePrecipitationForecast(hour, value);

        // Assert
        act.ShouldThrow<ArgumentException>();
    }

    [Fact]
    public void DefinePrecipitationForecast_WithDuplicateHours_ShouldReplace()
    {
        // Arrange
        var forecast = TestForecast.Default(TestLocation.Default);
        forecast.DefinePrecipitationForecast(5, 100);

        // Act
        forecast.DefinePrecipitationForecast(5, 200);

        // Assert
        forecast.Precipitations.Count.ShouldBe(1);
        forecast.Precipitations.First().Hour.ShouldBe(5);
        forecast.Precipitations.First().Value.ShouldBe(200);
    }
}
