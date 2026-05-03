using Kimo.ZLApp.Domain.Locations;

namespace Kimo.ZLApp.Tests.Common.Locations;

public static class TestLocation
{
    public static Location Default => GenerateLocation("Test Location", 1);

    public static Location Alternative => GenerateLocation("Alternative Location", 2);

    public static Location Invalid => GenerateLocation("Invalid Location", 0);

    private static Location GenerateLocation(string name, int id)
    {
        var location = new Location(name);

        var idProperty = location.GetType().GetProperty(nameof(Location.Id));
        idProperty?.SetValue(location, id);

        return location;
    }
}
