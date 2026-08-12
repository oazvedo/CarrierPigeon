using BirdMessage.Application.Dto;
using BirdMessage.Application.Services.Interface;

namespace BirdMessage.Application.Services;

public class DeliveryEstimationService : IDeliveryEstimationService
{
    private const double EarthRadiusKm = 6371.0;

    public decimal CalculateDistanceKm(decimal originLatitude, decimal originLongitude, decimal destinationLatitude, decimal destinationLongitude)
    {
        var originLatRad = ToRadians((double)originLatitude);
        var destinationLatRad = ToRadians((double)destinationLatitude);
        var deltaLat = ToRadians((double)(destinationLatitude - originLatitude));
        var deltaLon = ToRadians((double)(destinationLongitude - originLongitude));

        var a = Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) +
                Math.Cos(originLatRad) * Math.Cos(destinationLatRad) *
                Math.Sin(deltaLon / 2) * Math.Sin(deltaLon / 2);

        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        return (decimal)(EarthRadiusKm * c);
    }

    public TimeSpan CalculateDuration(decimal distanceKm, decimal velocityKmh)
    {
        if (velocityKmh <= 0)
            throw new ArgumentOutOfRangeException(nameof(velocityKmh), "Bird velocity must be greater than zero.");

        return TimeSpan.FromHours((double)(distanceKm / velocityKmh));
    }

    public DeliveryEstimateDto Estimate(
        decimal originLatitude,
        decimal originLongitude,
        decimal destinationLatitude,
        decimal destinationLongitude,
        decimal velocityKmh)
    {
        var distanceKm = CalculateDistanceKm(originLatitude, originLongitude, destinationLatitude, destinationLongitude);
        var duration = CalculateDuration(distanceKm, velocityKmh);

        return new DeliveryEstimateDto(distanceKm, duration);
    }

    private static double ToRadians(double degrees) => degrees * Math.PI / 180;
}
