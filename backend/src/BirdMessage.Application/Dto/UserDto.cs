using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BirdMessage.Domain.Enums;

namespace BirdMessage.Application.Dto
{
    public record CreateUserRequestDto(string name, string email, string password, Role role);

    public record UpdateUserRequestDto(string name, string email, string password, Role role);

    public record UserResponseDto(Guid Id, string name, string email, DateTime createdAt, DateTime updatedAt, Role role);
}