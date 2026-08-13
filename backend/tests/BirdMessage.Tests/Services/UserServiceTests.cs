using BirdMessage.Application.Services;
using BirdMessage.Domain.Common;
using BirdMessage.Domain.Entities;
using BirdMessage.Domain.Interfaces;
using Moq;
using Xunit;

namespace BirdMessage.Tests.Services;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _repository = new();
    private readonly Mock<IPasswordHasher> _passwordHasher = new();
    private readonly UserService _service;

    public UserServiceTests()
    {
        _service = new UserService(_repository.Object, _passwordHasher.Object);
    }

    [Fact]
    public async Task GetByIdAsync_DelegatesToRepository()
    {
        var user = new User { Id = Guid.NewGuid(), Name = "Ana" };
        _repository.Setup(r => r.GetByIdAsync(user.Id, It.IsAny<CancellationToken>())).ReturnsAsync(user);

        var result = await _service.GetByIdAsync(user.Id);

        Assert.Same(user, result);
    }

    [Fact]
    public async Task GetPagedAsync_DelegatesToRepository()
    {
        var page = new PaginatedResult<User>([new User { Name = "Bia" }], 1, 1, 10);
        _repository.Setup(r => r.GetPagedAsync(1, 10, It.IsAny<CancellationToken>())).ReturnsAsync(page);

        var result = await _service.GetPagedAsync(1, 10);

        Assert.Same(page, result);
    }

    [Fact]
    public async Task CreateAsync_HashesPasswordSetsTimestampsAndPersists()
    {
        var user = new User { Name = "Carlos", Email = "carlos@teste.com", Password = "plain-password" };
        _passwordHasher.Setup(h => h.Hash("plain-password")).Returns("hashed-password");

        var result = await _service.CreateAsync(user);

        Assert.Equal("hashed-password", result.Password);
        Assert.NotEqual(default, result.CreatedAt);
        Assert.NotEqual(default, result.UpdatedAt);
        _passwordHasher.Verify(h => h.Hash("plain-password"), Times.Once);
        _repository.Verify(r => r.AddAsync(user, It.IsAny<CancellationToken>()), Times.Once);
        _repository.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_HashesPasswordUpdatesTimestampAndPersists()
    {
        var user = new User { Id = Guid.NewGuid(), Name = "Diana", Password = "plain-password" };
        _passwordHasher.Setup(h => h.Hash("plain-password")).Returns("hashed-password");

        await _service.UpdateAsync(user);

        Assert.Equal("hashed-password", user.Password);
        Assert.NotEqual(default, user.UpdatedAt);
        _passwordHasher.Verify(h => h.Hash("plain-password"), Times.Once);
        _repository.Verify(r => r.Update(user), Times.Once);
        _repository.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ExistingUser_DeletesAndPersists()
    {
        var user = new User { Id = Guid.NewGuid(), Name = "Elis" };
        _repository.Setup(r => r.GetByIdAsync(user.Id, It.IsAny<CancellationToken>())).ReturnsAsync(user);

        await _service.DeleteAsync(user.Id);

        _repository.Verify(r => r.Delete(user), Times.Once);
        _repository.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_MissingUser_DoesNothing()
    {
        var id = Guid.NewGuid();
        _repository.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync((User?)null);

        await _service.DeleteAsync(id);

        _repository.Verify(r => r.Delete(It.IsAny<User>()), Times.Never);
        _repository.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
