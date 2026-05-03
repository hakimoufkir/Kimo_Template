using System.Diagnostics.CodeAnalysis;
using Kimo.ZLApp.Application.Locations.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Kimo.ZLApp.Web.Common.Swagger.Examples.Locations;

/// <inheritdoc />
[ExcludeFromCodeCoverage]
public class LocationDtoListExample : IExamplesProvider<List<LocationDto>>
{
    /// <inheritdoc />
    public List<LocationDto> GetExamples()
    {
        return [new LocationDtoExample().GetExamples()];
    }
}