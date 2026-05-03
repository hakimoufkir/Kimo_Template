using Kimo.ZLApp.Domain.Common;
using Kimo.ZLApp.Domain.Locations;

namespace Kimo.ZLApp.Domain.WeatherForecasts;

public sealed class Forecast : IAggregateRoot
{
    private readonly List<PrecipitationEntry> _precipitations = [];

    private Forecast()
    {
    }

    public Forecast(DateOnly date, Location location, DateOnly currentDate)
    {
        ValidateDate(date, currentDate);
        ValidateLocation(location);

        Date = date;
        LocationId = location.Id;
        Location = location;
    }

    public int Id { get; private set; }

    public DateOnly Date { get; private set; }

    public int LocationId { get; private set; }

    public Location? Location { get; private set; }

    public TemperatureData TemperatureData { get; } = new();

    public IReadOnlyCollection<PrecipitationEntry> Precipitations => _precipitations.AsReadOnly();

    public void DefinePrecipitationForecast(int hour, double value)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(hour, 0);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(hour, 23);
        ArgumentOutOfRangeException.ThrowIfLessThan(value, 0);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(value, 1000);

        var existingEntry = _precipitations.FirstOrDefault(v => v.Hour == hour);

        if (existingEntry != null)
        {
            _precipitations.Remove(existingEntry);
        }

        _precipitations.Add(new PrecipitationEntry(hour, value));
    }

    public void DefineTemperatureForecast(int hour, double value)
    {
        TemperatureData.DefineTemperatureForecast(hour, value);
    }

    public void ChangeLocation(Location location)
    {
        ValidateLocation(location);

        LocationId = location.Id;
        Location = location;
    }

    private static void ValidateLocation(Location location)
    {
        ArgumentNullException.ThrowIfNull(location);

        if (location.Id == 0)
        {
            throw new ArgumentException("Location must have an Id.", nameof(location));
        }
    }

    private static void ValidateDate(DateOnly date, DateOnly currentDate)
    {
        if (date < currentDate)
        {
            throw new ArgumentException("Date cannot be in the past.", nameof(date));
        }
    }
}
