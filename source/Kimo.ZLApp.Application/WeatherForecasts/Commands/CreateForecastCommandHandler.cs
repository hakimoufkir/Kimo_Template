using Microsoft.Extensions.Logging;
using Kimo.ZLApp.Application.Common;
using Kimo.ZLApp.Application.Common.RequestPipelines;
using Kimo.ZLApp.Application.Common.Results;
using Kimo.ZLApp.Application.Common.Results.Error;
using Kimo.ZLApp.Application.Common.Results.ResultModels;
using Kimo.ZLApp.Application.Common.TimeProvider;
using Kimo.ZLApp.Application.Locations;
using Kimo.ZLApp.Application.WeatherForecasts.Mappers;
using Kimo.ZLApp.Application.WeatherForecasts.Models;
using Kimo.ZLApp.Domain.Locations;
using Kimo.ZLApp.Domain.WeatherForecasts;

namespace Kimo.ZLApp.Application.WeatherForecasts.Commands;

public sealed class CreateForecastCommandHandler(
    IForecastRepository forecastRepo,
    ILocationRepository locationRepo,
    IUnitOfWork unitOfWork,
    TimeProvider timeProvider,
    ILogger<CreateForecastCommandHandler> logger
)
    : IRequestHandler<CreateForecastCommand, Result<ForecastDto>>
{
    public async Task<Result<ForecastDto>> HandleAsync(
        CreateForecastCommand request,
        CancellationToken cancellationToken = default
    )
    {
        var location = await locationRepo.FindAsync(request.LocationId, cancellationToken);
        if (location is null)
        {
            logger.LocationNotFound(request.LocationId);
            return Error.DependencyNotFound(nameof(Location));
        }

        var forecast =
            await forecastRepo.GetByDateForLocationAsync(request.LocationId, request.Date, cancellationToken);

        if (forecast is null)
        {
            forecast = new Forecast(request.Date, location, timeProvider.GetLocalToday());
            forecastRepo.Add(forecast);

            logger.CreateNewForecast(request.LocationId, request.Date);
        }

        if (request.Precipitation is not null)
        {
            forecast.DefinePrecipitationForecast(request.Hour, (double)request.Precipitation);
            logger.AddingPrecipitation(request.Hour, (double)request.Precipitation);
        }

        if (request.Temperature is not null)
        {
            forecast.DefineTemperatureForecast(request.Hour, (double)request.Temperature);
            logger.AddingTemperature(request.Hour, (double)request.Temperature);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        logger.CreatedSuccessfully(forecast.Id);

        return new CreatedResult<ForecastDto>($"/forecasts/{forecast.Id}", forecast.ToDto());
    }
}