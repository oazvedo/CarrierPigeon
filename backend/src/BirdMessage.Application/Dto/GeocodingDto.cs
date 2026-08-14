namespace BirdMessage.Application.Dto;

public record CoordinatesDto(
    decimal Latitude,
    decimal Longitude);

public record GeocodingResponseDto(
    decimal Latitude,
    decimal Longitude,
    decimal? Distance = null);
