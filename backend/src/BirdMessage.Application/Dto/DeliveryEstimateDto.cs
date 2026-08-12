namespace BirdMessage.Application.Dto;

public record DeliveryEstimateDto(decimal DistanceKm, TimeSpan EstimatedDuration);
