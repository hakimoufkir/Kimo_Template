using Kimo.ZLApp.Application.Common;
using Kimo.ZLApp.Application.Common.Filtering.Requests;
using Kimo.ZLApp.Application.Common.Filtering.Responses;
using Kimo.ZLApp.Domain.WeatherForecasts;

namespace Kimo.ZLApp.Application.WeatherForecasts;

public interface IForecastRepository : IRepositoryBase<Forecast, int>
{
    Task<PaginatedData<Forecast>> GetAllAsync(PaginationOptions pagination, SortingOptions sorting,
        FilterOptions? filter, CancellationToken cancellationToken = default);

    Task<Forecast?> GetByDateForLocationAsync(int locationId, DateOnly forecastDate,
        CancellationToken cancellationToken = default);

    Task<int> RemoveAllBeforeDateAsync(DateOnly maxLifetimeDate, CancellationToken cancellationToken = default);
}