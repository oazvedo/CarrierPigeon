using BirdMessage.Application.Services;
using BirdMessage.Domain.Common;
using BirdMessage.Domain.Entities;
using BirdMessage.Domain.Interfaces;
using Moq;
using Xunit;

namespace BirdMessage.Tests.Services;

public class BirdServiceTests
{
    private readonly Mock<IBirdRepository> _repository = new();
    private readonly BirdService _service;

    public BirdServiceTests()
    {
        _service = new BirdService(_repository.Object);
    }

    [Fact]
    public async Task GetByIdAsync_DelegatesToRepository()
    {
        var bird = new Bird { Id = Guid.NewGuid(), Name = "Pombo" };
        _repository.Setup(r => r.GetByIdAsync(bird.Id, It.IsAny<CancellationToken>())).ReturnsAsync(bird);

        var result = await _service.GetByIdAsync(bird.Id);

        Assert.Same(bird, result);
    }

    [Fact]
    public async Task GetPagedAsync_DelegatesToRepository()
    {
        var page = new PaginatedResult<Bird>([new Bird { Name = "Coruja" }], 1, 1, 10);
        _repository.Setup(r => r.GetPagedAsync(1, 10, It.IsAny<CancellationToken>())).ReturnsAsync(page);

        var result = await _service.GetPagedAsync(1, 10);

        Assert.Same(page, result);
    }

    [Fact]
    public async Task CreateAsync_SetsTimestampsAndPersists()
    {
        var bird = new Bird { Name = "Urubu", Description = "Chega em momentos péssimos", Velocity = 40 };

        var result = await _service.CreateAsync(bird);

        Assert.NotEqual(default, result.CreatedAt);
        Assert.NotEqual(default, result.UpdatedAt);
        _repository.Verify(r => r.AddAsync(bird, It.IsAny<CancellationToken>()), Times.Once);
        _repository.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesTimestampAndPersists()
    {
        var bird = new Bird { Id = Guid.NewGuid(), Name = "Pavão" };

        await _service.UpdateAsync(bird);

        Assert.NotEqual(default, bird.UpdatedAt);
        _repository.Verify(r => r.Update(bird), Times.Once);
        _repository.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ExistingBird_DeletesAndPersists()
    {
        var bird = new Bird { Id = Guid.NewGuid(), Name = "Galinha" };
        _repository.Setup(r => r.GetByIdAsync(bird.Id, It.IsAny<CancellationToken>())).ReturnsAsync(bird);

        await _service.DeleteAsync(bird.Id);

        _repository.Verify(r => r.Delete(bird), Times.Once);
        _repository.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_MissingBird_DoesNothing()
    {
        var id = Guid.NewGuid();
        _repository.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync((Bird?)null);

        await _service.DeleteAsync(id);

        _repository.Verify(r => r.Delete(It.IsAny<Bird>()), Times.Never);
        _repository.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
