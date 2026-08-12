using BirdMessage.Application.Dto;
using BirdMessage.Application.Mappers;
using BirdMessage.Application.Services.Interface;
using BirdMessage.Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace BirdMessage.API.Controllers;

[ApiController]
[Route("api/birds")]
public class BirdsController(IBirdService birdService) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BirdResponseDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var bird = await birdService.GetByIdAsync(id, cancellationToken);
        return bird is null ? NotFound() : Ok(bird.ToResponseDto());
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedResult<BirdResponseDto>>> GetPaged(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        if (page < 1 || pageSize < 1)
            return BadRequest("page and pageSize must be greater than zero.");

        var result = await birdService.GetPagedAsync(page, pageSize, cancellationToken);
        return Ok(result.ToResponseDto());
    }

    [HttpPost]
    public async Task<ActionResult<BirdResponseDto>> Create(CreateBirdRequestDto request, CancellationToken cancellationToken)
    {
        var bird = await birdService.CreateAsync(request.ToEntity(), cancellationToken);
        var response = bird.ToResponseDto();
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateBirdRequestDto request, CancellationToken cancellationToken)
    {
        var bird = await birdService.GetByIdAsync(id, cancellationToken);
        if (bird is null) return NotFound();

        request.ApplyTo(bird);
        await birdService.UpdateAsync(bird, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await birdService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
