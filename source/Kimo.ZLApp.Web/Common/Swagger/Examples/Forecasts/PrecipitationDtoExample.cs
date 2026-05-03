using System.Diagnostics.CodeAnalysis;
using Kimo.ZLApp.Application.WeatherForecasts.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Kimo.ZLApp.Web.Common.Swagger.Examples.Forecasts;

/// <inheritdoc />
[ExcludeFromCodeCoverage]
public class PrecipitationDtoExample : IExamplesProvider<PrecipitationDto>
{
    /// <inheritdoc />
    public PrecipitationDto GetExamples()
    {
        return new PrecipitationDto(15, 15.3);
    }
}