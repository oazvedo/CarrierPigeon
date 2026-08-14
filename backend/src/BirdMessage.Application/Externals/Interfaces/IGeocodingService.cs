using BirdMessage.Application.Dto;

namespace BirdMessage.Application.Externals.Interfaces;

public interface IGeocodingService
{
    Task<GeocodingRouteResultDto> GetRouteAsync(
        GeocodingRoutePointDto origin,
        GeocodingRoutePointDto destination,
        CancellationToken cancellationToken = default);
}
