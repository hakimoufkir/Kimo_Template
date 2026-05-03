namespace Kimo.ZLApp.Domain.WeatherForecasts;

public sealed class TemperatureData
{
    private readonly List<TemperatureDataEntry> _temperatures = [];

    public IReadOnlyCollection<TemperatureDataEntry> Temperatures => _temperatures.AsReadOnly();

    public TemperatureDataEntry? HottestEntry => Temperatures.MaxBy(t => t.Value);

    internal void DefineTemperatureForecast(int hour, double value)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(hour, 0);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(hour, 23);
        ArgumentOutOfRangeException.ThrowIfLessThan(value, -100);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(value, 100);

        var existingEntry = _temperatures.FirstOrDefault(t => t.Hour == hour);

        if (existingEntry is not null)
        {
            _temperatures.Remove(existingEntry);
        }

        _temperatures.Add(new TemperatureDataEntry(hour, value));
    }
}