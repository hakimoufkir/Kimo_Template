using Microsoft.Extensions.Logging;
using Kimo.ZLApp.Application.Common.RequestPipelines;
using Kimo.ZLApp.Application.Common.Results;
using Kimo.ZLApp.Application.Locations.Mappers;
using Kimo.ZLApp.Application.Locations.Models;

namespace Kimo.ZLApp.Application.Locations.Queries;

public sealed class AllLocationsQueryHandler(ILocationRepository repository, ILogger<AllLocationsQueryHandler> logger)
    : IRequestHandler<AllLocationsQuery, Result<List<LocationDto>>>
{
    public async Task<Result<List<LocationDto>>> HandleAsync(AllLocationsQuery request,
        CancellationToken cancellationToken = default)
    {
        var allLocations = await repository.GetAllAsync(cancellationToken);
        logger.SuccessfullyLoadedLocations(allLocations.Count);

        return allLocations.ToDto();
    }
}