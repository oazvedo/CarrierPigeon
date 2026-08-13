using BirdMessage.Application.Dto;
using BirdMessage.Application.Mappers;
using BirdMessage.Application.Services.Interface;
using BirdMessage.Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace BirdMessage.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController(IUserService userService) : ControllerBase
    {
        [HttpGet("{id:guid}")]
        public async Task <ActionResult<UserResponseDto>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var user = await userService.GetByIdAsync(id, cancellationToken);
            return user is null ? NotFound() : Ok(user.ToResponseDto());
        }

        [HttpGet]
        public async Task <ActionResult<PaginatedResult<UserResponseDto>>> GetPaginated(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default
        )
        {
            if (page < 1 || pageSize < 1)
            return BadRequest("page and pageSize must be greater than zero.");

            var result = await userService.GetPagedAsync(page, pageSize, cancellationToken);
            return Ok(result.ToResponseDto());
        }

        [HttpPost]
        public async Task <ActionResult<UserResponseDto>> Create(CreateUserRequestDto dto, CancellationToken cancellationToken)
        {
            var user = await userService.CreateAsync(dto.ToEntity(), cancellationToken);
            var response = user.ToResponseDto();
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        [HttpPut("{id:guid}")]
        public async Task <IActionResult> Update(Guid id, UpdateUserRequestDto dto, CancellationToken cancellationToken)
        {
            var user = await userService.GetByIdAsync(id, cancellationToken);
            if (user is null) return NotFound();

            dto.ApplyTo(user);
            await userService.UpdateAsync(user);
            return NoContent();
        }


        [HttpDelete("{id:guid}")]
        public async Task <IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
             await userService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
    }
}