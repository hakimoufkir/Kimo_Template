using Kimo.ZLApp.Application.Locations.Models;
using Kimo.ZLApp.Domain.Locations;

namespace Kimo.ZLApp.Application.Locations.Mappers;

public static class LocationMapper
{
    public static LocationDto ToDto(this Location location)
    {
        return new LocationDto(location.Id, location.Name);
    }

    public static List<LocationDto> ToDto(this IEnumerable<Location> locations)
    {
        return locations.Select(l => l.ToDto()).ToList();
    }
}