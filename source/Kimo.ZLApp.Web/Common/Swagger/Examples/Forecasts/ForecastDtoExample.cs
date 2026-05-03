using System.Diagnostics.CodeAnalysis;
using Kimo.ZLApp.Application.WeatherForecasts.Models;
using Kimo.ZLApp.Web.Common.Swagger.Examples.Locations;
using Swashbuckle.AspNetCore.Filters;

namespace Kimo.ZLApp.Web.Common.Swagger.Examples.Forecasts;

/// <inheritdoc />
[ExcludeFromCodeCoverage]
public class ForecastDtoExample : IExamplesProvider<ForecastDto>
{
    /// <inheritdoc />
    public ForecastDto GetExamples()
    {
        return new ForecastDto(2,
            "2025-08-01",
            new LocationDtoExample().GetExamples(),
            [
                new TemperatureDtoExample().GetExamples()
            ],
            [
                new PrecipitationDtoExample().GetExamples()
            ]
        );
    }
}