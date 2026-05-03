using Microsoft.EntityFrameworkCore;
using Kimo.ZLApp.Application.Locations;
using Kimo.ZLApp.Domain.Locations;
using Kimo.ZLApp.Infrastructure.Common;
using Kimo.ZLApp.Infrastructure.Common.Database;

namespace Kimo.ZLApp.Infrastructure.Locations;

public class LocationRepository(IApplicationDbContext dbContext)
    : RepositoryBase<Location, int>(dbContext), ILocationRepository
{
    public new async Task<IReadOnlyCollection<Location>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await DbSet.OrderBy(l => l.Name).ToListAsync(cancellationToken);
    }
}
