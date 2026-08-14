using System.Text.Json.Serialization;

namespace BirdMessage.Application.Dto;

public record CoordinatesDto(
    decimal Latitude,
    decimal Longitude);

public record GeocodingRouteRequestDto(
    List<GeocodingRoutePointDto> Route);

public record GeocodingRoutePointDto(
    string Name,
    string Country);

public record GeocodingRouteResponseDto(
    List<GeocodingFeatureDto> Points,
    GeocodingRouteDistanceDto Route);

public record GeocodingFeatureDto(
    GeocodingGeometryDto Geometry);

public record GeocodingGeometryDto(
    [property: JsonPropertyName("coordinates")] List<decimal> Coordinates);

public record GeocodingRouteDistanceDto(
    decimal Vincenty);

public record GeocodingRouteResultDto(
    CoordinatesDto Origin,
    CoordinatesDto Destination,
    decimal DistanceKm);
