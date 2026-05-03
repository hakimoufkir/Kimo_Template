using System.Diagnostics.CodeAnalysis;
using Kimo.ZLApp.Application.Locations.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Kimo.ZLApp.Web.Common.Swagger.Examples.Locations;

/// <inheritdoc />
[ExcludeFromCodeCoverage]
public class LocationDtoExample : IExamplesProvider<LocationDto>
{
    /// <inheritdoc />
    public LocationDto GetExamples()
    {
        return new LocationDto(1, "Munich");
    }
}