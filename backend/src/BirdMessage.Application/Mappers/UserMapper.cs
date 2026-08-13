using BirdMessage.Application.Dto;
using BirdMessage.Domain.Common;
using BirdMessage.Domain.Entities;

namespace BirdMessage.Application.Mappers
{
    public static class UserMapper
    {
        public static UserResponseDto ToResponseDto(this User user)
        => new(user.Id, user.Name, user.Email, user.CreatedAt, user.UpdatedAt, user.Role);

        
        public static PaginatedResult<UserResponseDto> ToResponseDto(this PaginatedResult<User> result)
        => new(result.Items.Select(ToResponseDto).ToList(), result.TotalCount, result.Page, result.PageSize);


        public static User ToEntity(this CreateUserRequestDto dto)
        => new()
        {
            Id = Guid.NewGuid(),
            Name = dto.name,
            Email = dto.email,
            Password = dto.password,
            Role = dto.role,
        };

        public static void ApplyTo(this UpdateUserRequestDto dto, User user)
        {
            user.Name = dto.name;
            user.Email = dto.email;
            user.Password = dto.password;
            user.Role = dto.role;
        }

    }
}