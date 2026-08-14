using System.Text;
using System.Text.Json;
using BirdMessage.Application.Dto;
using BirdMessage.Application.Externals.Interfaces;
using Microsoft.Extensions.Configuration;

namespace BirdMessage.Application.Externals
{
    public class GeocodingService(HttpClient client, IConfiguration configuration) : IGeocodingService
    {
        private static readonly JsonSerializerOptions SerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public async Task<GeocodingRouteResultDto> GetRouteAsync(
            GeocodingRoutePointDto origin,
            GeocodingRoutePointDto destination,
            CancellationToken cancellationToken = default)
        {
            var baseUrl = configuration["ExternalApis:DistanceApi"]
                ?? throw new InvalidOperationException("ExternalApis:DistanceApi configuration is missing.");
            var token = configuration["ExternalApis:DistanceApiToken"]
                ?? throw new InvalidOperationException("ExternalApis:DistanceApiToken configuration is missing.");

            var requestDto = new GeocodingRouteRequestDto([origin, destination]);
            var requestJson = JsonSerializer.Serialize(requestDto, SerializerOptions);

            var url = $"{baseUrl.TrimEnd('/')}/distance/route";

            using var requestMessage = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(requestJson, Encoding.UTF8, "application/json")
            };
            requestMessage.Headers.Add("X-Billing-Token", token);

            using var response = await client.SendAsync(requestMessage, cancellationToken);
            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync(cancellationToken);
            var result = JsonSerializer.Deserialize<GeocodingRouteResponseDto>(responseJson, SerializerOptions)
                ?? throw new InvalidOperationException("Unable to deserialize geocoding response.");

            if (result.Points is not { Count: 2 })
            {
                throw new InvalidOperationException("Geocoding response did not return the expected number of points.");
            }

            var originCoordinates = ToCoordinates(result.Points[0]);
            var destinationCoordinates = ToCoordinates(result.Points[1]);

            return new GeocodingRouteResultDto(originCoordinates, destinationCoordinates, result.Route.Vincenty);
        }

        private static CoordinatesDto ToCoordinates(GeocodingFeatureDto feature)
        {
            var coordinates = feature.Geometry.Coordinates;
            if (coordinates is not { Count: 2 })
            {
                throw new InvalidOperationException("Geocoding response point did not contain coordinates.");
            }

            var longitude = coordinates[0];
            var latitude = coordinates[1];
            return new CoordinatesDto(latitude, longitude);
        }
    }
}
