using Kimo.ZLApp.Domain.Common;

namespace Kimo.ZLApp.Domain.Locations;

public sealed class Location : IAggregateRoot
{
    public const int NameMaxLength = 100;

    private Location()
    {
    }

    public Location(string name)
    {
        ValidateName(name);
        Name = name;
    }

    public int Id { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public void Rename(string name)
    {
        ValidateName(name);
        Name = name;
    }

    private static void ValidateName(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(name.Length, NameMaxLength, nameof(name));
    }
}