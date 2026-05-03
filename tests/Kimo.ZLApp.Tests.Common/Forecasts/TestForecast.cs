using Kimo.ZLApp.Domain.Locations;
using Kimo.ZLApp.Domain.WeatherForecasts;

namespace Kimo.ZLApp.Tests.Common.Forecasts;

public static class TestForecast
{
    public static Forecast Default(Location location) =>
        GenerateForecast(location, new DateOnly(2026, 1, 5), 1, [], []);

    public static Forecast Alternative(Location location) => GenerateForecast(location, new DateOnly(2026, 1, 6), 2,
        [(9, 20), (12, 22), (15, 22)],
        [(6, 0), (9, 0), (12, 10), (15, 60), (18, 40)]);

    private static Forecast GenerateForecast(Location location, DateOnly date, int id,
        ICollection<(int Hour, int Value)> temperatures,
        ICollection<(int Hour, int Value)> precipitationEntries
    )
    {
        var forecast = new Forecast(date, location, date);

        var idProperty = forecast.GetType().GetProperty(nameof(Forecast.Id));
        idProperty?.SetValue(forecast, id);

        foreach (var temperature in temperatures)
        {
            forecast.DefineTemperatureForecast(temperature.Hour, temperature.Value);
        }

        foreach (var precipitationEntry in precipitationEntries)
        {
            forecast.DefinePrecipitationForecast(precipitationEntry.Hour, precipitationEntry.Value);
        }

        return forecast;
    }
}
