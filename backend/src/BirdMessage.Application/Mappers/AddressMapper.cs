using BirdMessage.Application.Dto;
using BirdMessage.Domain.Common;
using BirdMessage.Domain.Entities;

namespace BirdMessage.Application.Mappers;

public static class AddressMapper
{
    public static AddressResponseDto ToResponseDto(this Address address)
        => new(
            address.Id,
            address.UserId,
            address.Cep,
            address.Number,
            address.Street,
            address.Neighborhood,
            address.Local,
            address.Uf,
            address.State,
            address.Region,
            address.DDD);

    public static PaginatedResult<AddressResponseDto> ToResponseDto(this PaginatedResult<Address> result)
        => new(result.Items.Select(ToResponseDto).ToList(), result.TotalCount, result.Page, result.PageSize);

    public static Address ToEntity(this CreateAddressRequestDto dto)
        => new()
        {
            Id = Guid.NewGuid(),
            UserId = dto.UserId,
            Cep = dto.Cep,
            Number = dto.Number
        };

    public static GeocodingRoutePointDto ToGeocodingRoutePoint(this Address address)
    {
        var cepPrefix = address.Cep.Split('-')[0];

        var nameParts = new[] { $"{address.Street} {address.Number}", address.Neighborhood, address.Local, address.State, cepPrefix }
            .Where(part => !string.IsNullOrWhiteSpace(part))
            .Select(part => part!.Trim().Replace(' ', '-'))
            .Append("BRA");

        var name = string.Join(", ", nameParts);
        return new GeocodingRoutePointDto(name, "Brazil");
    }

    public static void ApplyTo(this UpdateAddressRequestDto dto, Address address)
    {
        address.Cep = dto.Cep;
        address.Number = dto.Number;
        address.Street = dto.Street;
        address.Neighborhood = dto.Neighborhood;
        address.Local = dto.Local;
        address.Uf = dto.Uf;
        address.State = dto.State;
        address.Region = dto.Region;
        address.DDD = dto.DDD;
    }
}
