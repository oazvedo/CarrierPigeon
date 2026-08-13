using BirdMessage.Application.Services.Interface;
using BirdMessage.Domain.Common;
using BirdMessage.Domain.Entities;
using BirdMessage.Domain.Interfaces;

namespace BirdMessage.Application.Services;

public class BirdService(IBirdRepository birdRepository) : IBirdService
{
    public Task<Bird?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => birdRepository.GetByIdAsync(id, cancellationToken);

    public Task<PaginatedResult<Bird>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
        => birdRepository.GetPagedAsync(page, pageSize, cancellationToken);

    public async Task<Bird> CreateAsync(Bird bird, CancellationToken cancellationToken = default)
    {
        bird.CreatedAt = DateTime.UtcNow;
        bird.UpdatedAt = DateTime.UtcNow;
        await birdRepository.AddAsync(bird, cancellationToken);
        await birdRepository.SaveChangesAsync(cancellationToken);
        return bird;
    }

    public async Task UpdateAsync(Bird bird, CancellationToken cancellationToken = default)
    {
        bird.UpdatedAt = DateTime.UtcNow;
        birdRepository.Update(bird);
        await birdRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var bird = await birdRepository.GetByIdAsync(id, cancellationToken);
        if (bird is null) return;

        birdRepository.Delete(bird);
        await birdRepository.SaveChangesAsync(cancellationToken);
    }
}
