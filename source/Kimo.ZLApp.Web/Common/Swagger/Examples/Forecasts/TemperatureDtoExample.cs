using System.Diagnostics.CodeAnalysis;
using Kimo.ZLApp.Application.WeatherForecasts.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Kimo.ZLApp.Web.Common.Swagger.Examples.Forecasts;

/// <inheritdoc />
[ExcludeFromCodeCoverage]
public class TemperatureDtoExample : IExamplesProvider<TemperatureDto>
{
    /// <inheritdoc />
    public TemperatureDto GetExamples()
    {
        return new TemperatureDto(11, 19.5);
    }
}