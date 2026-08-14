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
            Cep = dto.Cep
        };

    public static void ApplyTo(this UpdateAddressRequestDto dto, Address address)
    {
        address.Cep = dto.Cep;
        address.Street = dto.Street;
        address.Neighborhood = dto.Neighborhood;
        address.Local = dto.Local;
        address.Uf = dto.Uf;
        address.State = dto.State;
        address.Region = dto.Region;
        address.DDD = dto.DDD;
    }
}
