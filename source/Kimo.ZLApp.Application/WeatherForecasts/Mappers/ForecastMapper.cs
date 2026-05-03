using System.Globalization;
using Kimo.ZLApp.Application.Common.Filtering.Responses;
using Kimo.ZLApp.Application.Locations.Mappers;
using Kimo.ZLApp.Application.Locations.Models;
using Kimo.ZLApp.Application.WeatherForecasts.Models;
using Kimo.ZLApp.Domain.WeatherForecasts;

namespace Kimo.ZLApp.Application.WeatherForecasts.Mappers;

public static class ForecastMapper
{
    public static ForecastDto ToDto(this Forecast forecast)
    {
        return new ForecastDto(forecast.Id, forecast.Date.ToString("O", CultureInfo.CurrentCulture),
            forecast.Location?.ToDto() ?? new LocationDto(0, string.Empty),
            forecast.TemperatureData.Temperatures.Select(t => new TemperatureDto(t.Hour, t.Value)).ToArray(),
            forecast.Precipitations.Select(p => new PrecipitationDto(p.Hour, p.Value)).ToArray());
    }

    public static PaginatedData<ForecastDto> ToDto(this PaginatedData<Forecast> forecasts)
    {
        return new PaginatedData<ForecastDto>(forecasts.Data.Select(f => f.ToDto()).ToList(), forecasts.Page,
            forecasts.PageSize,
            forecasts.TotalRecords);
    }
}