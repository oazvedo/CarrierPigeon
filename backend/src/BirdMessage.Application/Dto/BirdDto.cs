namespace BirdMessage.Application.Dto;

public record BirdResponseDto(Guid Id, string Name, string Description, decimal Velocity, DateTime CreatedAt, DateTime UpdatedAt);

public record CreateBirdRequestDto(string Name, string Description, decimal Velocity);

public record UpdateBirdRequestDto(string Name, string Description, decimal Velocity);
