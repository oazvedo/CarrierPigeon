using BirdMessage.Application.Dto;
using BirdMessage.Domain.Common;
using BirdMessage.Domain.Entities;

namespace BirdMessage.Application.Mappers;

public static class BirdMapper
{
    public static BirdResponseDto ToResponseDto(this Bird bird)
        => new(bird.Id, bird.Name, bird.Description, bird.Velocity, bird.CreatedAt, bird.UpdatedAt);

    public static PaginatedResult<BirdResponseDto> ToResponseDto(this PaginatedResult<Bird> result)
        => new(result.Items.Select(ToResponseDto).ToList(), result.TotalCount, result.Page, result.PageSize);

    public static Bird ToEntity(this CreateBirdRequestDto dto)
        => new()
        {
            Name = dto.Name,
            Description = dto.Description,
            Velocity = dto.Velocity
        };

    public static void ApplyTo(this UpdateBirdRequestDto dto, Bird bird)
    {
        bird.Name = dto.Name;
        bird.Description = dto.Description;
        bird.Velocity = dto.Velocity;
    }
}
