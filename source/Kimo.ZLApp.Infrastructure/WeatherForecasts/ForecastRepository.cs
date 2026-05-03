using Microsoft.EntityFrameworkCore;
using Kimo.ZLApp.Application.Common.Filtering.Requests;
using Kimo.ZLApp.Application.Common.Filtering.Responses;
using Kimo.ZLApp.Application.WeatherForecasts;
using Kimo.ZLApp.Domain.WeatherForecasts;
using Kimo.ZLApp.Infrastructure.Common;
using Kimo.ZLApp.Infrastructure.Common.Database;
using Kimo.ZLApp.Infrastructure.Common.Filtering;
using Kimo.ZLApp.Infrastructure.Common.Filtering.Filters;
using Kimo.ZLApp.Infrastructure.Common.Filtering.Pagination;
using Kimo.ZLApp.Infrastructure.Common.Filtering.Sorting;

namespace Kimo.ZLApp.Infrastructure.WeatherForecasts;

public sealed class ForecastRepository : RepositoryBase<Forecast, int>, IForecastRepository
{
    private readonly QueryConfiguration<Forecast> _configuration;

    public ForecastRepository(IApplicationDbContext dbContext) : base(dbContext)
    {
        _configuration = new QueryConfiguration<Forecast>()
            .AddFilter(nameof(Forecast.LocationId), f => f.LocationId, FilterType.In)
            .AddFilter(nameof(Forecast.Location), f => f.Location!.Name, FilterType.Contains)
            .AddFilter(nameof(Forecast.Date), f => f.Date, FilterType.DateEquals)
            .AddSort(nameof(Forecast.Date), f => f.Date)
            .AddSort(nameof(Forecast.Location), f => f.Location!.Name);
    }

    public async Task<PaginatedData<Forecast>> GetAllAsync(PaginationOptions pagination, SortingOptions sorting,
        FilterOptions? filter, CancellationToken cancellationToken = default)
    {
        var query = DbSet
            .Include(f => f.Location)
            .AsSplitQuery()
            .AsNoTracking()
            .ApplyFilters(filter, _configuration)
            .ApplySort(sorting, _configuration);

        return await query.ToPaginatedAsync(pagination, cancellationToken);
    }

    public Task<Forecast?> GetByDateForLocationAsync(int locationId, DateOnly forecastDate,
        CancellationToken cancellationToken = default)
    {
        return DbSet
            .Where(f => f.Date == forecastDate)
            .Where(f => f.LocationId == locationId)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<int> RemoveAllBeforeDateAsync(DateOnly maxLifetimeDate, CancellationToken cancellationToken = default)
    {
        return DbSet.Where(f => f.Date < maxLifetimeDate).ExecuteDeleteAsync(cancellationToken);
    }
}
