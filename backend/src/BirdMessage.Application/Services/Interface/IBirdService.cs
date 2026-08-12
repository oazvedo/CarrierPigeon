using BirdMessage.Domain.Common;
using BirdMessage.Domain.Entities;

namespace BirdMessage.Application.Services.Interface;

public interface IBirdService
{
    Task<Bird?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PaginatedResult<Bird>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    Task<Bird> CreateAsync(Bird bird, CancellationToken cancellationToken = default);
    Task UpdateAsync(Bird bird, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
