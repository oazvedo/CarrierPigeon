using BirdMessage.Application.Dto;
using BirdMessage.Application.Mappers;
using BirdMessage.Application.Services.Interface;
using BirdMessage.Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace BirdMessage.API.Controllers
{
    [ApiController]
    [Route("api/addresses")]
    public class AddressesController(IAddressService addressService) : ControllerBase
    {
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<AddressResponseDto>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var address = await addressService.GetByIdAsync(id, cancellationToken);
            return address is null ? NotFound() : Ok(address.ToResponseDto());
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedResult<AddressResponseDto>>> GetPaged(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            if (page < 1 || pageSize < 1)
                return BadRequest("page and pageSize must be greater than zero.");

            var result = await addressService.GetPagedAsync(page, pageSize, cancellationToken);
            return Ok(result.ToResponseDto());
        }

        [HttpPost]
        public async Task<ActionResult<AddressResponseDto>> Create(CreateAddressRequestDto request, CancellationToken cancellationToken)
        {
            var address = request.ToEntity();
            var created = await addressService.CreateAsync(address, cancellationToken);
            var response = created.ToResponseDto();
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdateAddressRequestDto request, CancellationToken cancellationToken)
        {
            var address = await addressService.GetByIdAsync(id, cancellationToken);
            if (address is null) return NotFound();

            request.ApplyTo(address);
            await addressService.UpdateAsync(address, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await addressService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
