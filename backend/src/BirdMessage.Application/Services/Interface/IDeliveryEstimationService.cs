using BirdMessage.Application.Dto;

namespace BirdMessage.Application.Services.Interface;

public interface IDeliveryEstimationService
{
    decimal CalculateDistanceKm(decimal originLatitude, decimal originLongitude, decimal destinationLatitude, decimal destinationLongitude);
    TimeSpan CalculateDuration(decimal distanceKm, decimal velocityKmh);

    DeliveryEstimateDto Estimate(
        decimal originLatitude,
        decimal originLongitude,
        decimal destinationLatitude,
        decimal destinationLongitude,
        decimal velocityKmh);
}
