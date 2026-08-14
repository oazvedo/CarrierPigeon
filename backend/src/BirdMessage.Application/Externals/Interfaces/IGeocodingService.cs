using BirdMessage.Application.Dto;

namespace BirdMessage.Application.Externals.Interfaces;

public interface IGeocodingService
{
    Task<CoordinatesDto> GetCoordinatesAsync(string cep, string? street = null, string? city = null, CancellationToken cancellationToken = default);
    Task<decimal> CalculateDistanceAsync(CoordinatesDto origin, CoordinatesDto destination, CancellationToken cancellationToken = default);
}
