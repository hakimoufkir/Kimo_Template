using Kimo.ZLApp.Tests.Common.Forecasts;
using Kimo.ZLApp.Tests.Common.Locations;

namespace Kimo.ZLApp.Domain.UnitTests.WeatherForecasts;

public class ForecastTestsTemperatureData
{
    [Theory]
    [InlineData(10, 20)]
    [InlineData(0, -100)]
    [InlineData(23, 100)]
    public void AddTemperature_WithValidData_ShouldAdd(int hour, int value)
    {
        // Arrange
        var forecast = TestForecast.Default(TestLocation.Default);

        // Act
        forecast.DefineTemperatureForecast(hour, value);

        // Assert
        forecast.TemperatureData.Temperatures.Count.ShouldBe(1);
        forecast.TemperatureData.Temperatures.First().Hour.ShouldBe(hour);
        forecast.TemperatureData.Temperatures.First().Value.ShouldBe(value);
    }

    [Theory]
    [InlineData(-1, 20)]
    [InlineData(24, 20)]
    public void AddTemperature_WithInvalidHours_ShouldThrow(int hour, int value)
    {
        // Arrange
        var forecast = TestForecast.Default(TestLocation.Default);

        // Act
        var act = () => forecast.DefineTemperatureForecast(hour, value);

        // Assert
        act.ShouldThrow<ArgumentException>();
    }

    [Theory]
    [InlineData(5, -101)]
    [InlineData(5, 101)]
    public void AddTemperature_WithInvalidValue_ShouldThrow(int hour, int value)
    {
        // Arrange
        var forecast = TestForecast.Default(TestLocation.Default);

        // Act
        var act = () => forecast.DefineTemperatureForecast(hour, value);

        // Assert
        act.ShouldThrow<ArgumentException>();
    }

    [Fact]
    public void AddTemperature_WithDuplicateHours_ShouldReplace()
    {
        // Arrange
        var forecast = TestForecast.Default(TestLocation.Default);
        forecast.DefineTemperatureForecast(10, 20);

        // Act
        forecast.DefineTemperatureForecast(10, 35);

        // Assert
        forecast.TemperatureData.Temperatures.Count.ShouldBe(1);
        forecast.TemperatureData.Temperatures.First().Hour.ShouldBe(10);
        forecast.TemperatureData.Temperatures.First().Value.ShouldBe(35);
    }

    [Fact]
    public void GetHottestEntry_WithValues_ShouldReturnHottestDay()
    {
        // Arrange
        var forecast = TestForecast.Default(TestLocation.Default);

        forecast.DefineTemperatureForecast(5, 20);
        forecast.DefineTemperatureForecast(22, 17);
        forecast.DefineTemperatureForecast(16, 22);
        forecast.DefineTemperatureForecast(8, 18);
        forecast.DefineTemperatureForecast(19, 26);
        forecast.DefineTemperatureForecast(12, 23);

        // Act
        var hottestDay = forecast.TemperatureData.HottestEntry;

        // Assert
        hottestDay.ShouldNotBeNull();
        hottestDay.Hour.ShouldBe(19);
        hottestDay.Value.ShouldBe(26);
    }

    [Fact]
    public void GetHottestEntry_WithDuplicateValues_ShouldReturnFirstHottestDay()
    {
        // Arrange
        var forecast = TestForecast.Default(TestLocation.Default);

        forecast.DefineTemperatureForecast(5, 20);
        forecast.DefineTemperatureForecast(22, 17);
        forecast.DefineTemperatureForecast(20, 26);
        forecast.DefineTemperatureForecast(16, 22);
        forecast.DefineTemperatureForecast(8, 18);
        forecast.DefineTemperatureForecast(19, 26);
        forecast.DefineTemperatureForecast(12, 23);

        // Act
        var hottestDay = forecast.TemperatureData.HottestEntry;

        // Assert
        hottestDay.ShouldNotBeNull();
        hottestDay.Hour.ShouldBe(20);
        hottestDay.Value.ShouldBe(26);
    }

    [Fact]
    public void GetHottestEntry_WithoutValues_ShouldReturnNull()
    {
        // Arrange
        var forecast = TestForecast.Default(TestLocation.Default);

        // Act
        var hottestDay = forecast.TemperatureData.HottestEntry;

        // Assert
        hottestDay.ShouldBeNull();
    }

    [Fact]
    public void DefineTemperatureForecast_AddEntry_ShouldAdd()
    {
        // Arrange
        var forecast = TestForecast.Default(TestLocation.Default);

        // Act
        forecast.DefineTemperatureForecast(10, 100);

        // Assert
        forecast.TemperatureData.Temperatures.Count.ShouldBe(1);
        forecast.TemperatureData.Temperatures.First().Hour.ShouldBe(10);
        forecast.TemperatureData.Temperatures.First().Value.ShouldBe(100);
    }
}
