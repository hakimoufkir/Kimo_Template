using System.Diagnostics.CodeAnalysis;
using Kimo.ZLApp.Application.Common.Filtering.Responses;
using Kimo.ZLApp.Application.WeatherForecasts.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Kimo.ZLApp.Web.Common.Swagger.Examples.Forecasts;

/// <inheritdoc />
[ExcludeFromCodeCoverage]
public class ForecastDtoPaginatedExample : IExamplesProvider<PaginatedData<ForecastDto>>
{
    /// <inheritdoc />
    public PaginatedData<ForecastDto> GetExamples()
    {
        return new PaginatedData<ForecastDto>([new ForecastDtoExample().GetExamples()], 0, 10, 1);
    }
}