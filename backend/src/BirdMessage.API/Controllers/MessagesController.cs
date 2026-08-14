using BirdMessage.Application.Dto;
using BirdMessage.Application.Mappers;
using BirdMessage.Application.Services.Interface;
using BirdMessage.Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace BirdMessage.API.Controllers
{
    [ApiController]
    [Route("api/messages")]
    public class MessagesController(IMessageService messageService) : ControllerBase
    {
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<MessageResponseDto>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var message = await messageService.GetByIdAsync(id, cancellationToken);
            return message is null ? NotFound() : Ok(message.ToResponseDto());
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedResult<MessageResponseDto>>> GetPaged(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            if (page < 1 || pageSize < 1)
                return BadRequest("page and pageSize must be greater than zero.");

            var result = await messageService.GetPagedAsync(page, pageSize, cancellationToken);
            return Ok(result.ToResponseDto());
        }

        [HttpPost]
        public async Task<ActionResult<MessageResponseDto>> Create(CreateMessageRequestDto request, CancellationToken cancellationToken)
        {
            var message = await messageService.CreateAsync(request.ToEntity(), cancellationToken);
            var response = message.ToResponseDto();
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdateMessageRequestDto request, CancellationToken cancellationToken)
        {
            var message = await messageService.GetByIdAsync(id, cancellationToken);
            if (message is null) return NotFound();

            request.ApplyTo(message);
            await messageService.UpdateAsync(message, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await messageService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
