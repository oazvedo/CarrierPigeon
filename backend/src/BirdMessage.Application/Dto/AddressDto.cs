namespace BirdMessage.Application.Dto;

public record AddressResponseDto(
    Guid Id,
    Guid UserId,
    string Cep,
    string? Number,
    string? Street,
    string? Neighborhood,
    string? Local,
    string? Uf,
    string? State,
    string? Region,
    string? DDD);

public record CreateAddressRequestDto(
    Guid UserId,
    string Cep,
    string? Number
);

public record UpdateAddressRequestDto(
    Guid UserId,
    string Cep,
    string? Number);