namespace BirdMessage.Application.Dto;

public record AddressResponseDto(
    Guid Id,
    Guid UserId,
    string Cep,
    string? Street,
    string? Neighborhood,
    string? Local,
    string? Uf,
    string? State,
    string? Region,
    string? DDD);

public record CreateAddressRequestDto(
    Guid UserId,
    string Cep
);

public record UpdateAddressRequestDto(
    string Cep,
    string? Street,
    string? Neighborhood,
    string? Local,
    string? Uf,
    string? State,
    string? Region,
    string? DDD);