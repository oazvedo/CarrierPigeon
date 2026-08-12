using BirdMessage.Domain.Entities;
using BirdMessage.Infrastructure.Data;
using BirdMessage.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BirdMessage.Tests.Repositories;

public class BirdRepositoryTests
{
    private static AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task AddAsync_ThenGetByIdAsync_ReturnsPersistedBird()
    {
        await using var context = CreateContext();
        var repository = new BirdRepository(context);
        var bird = new Bird { Id = Guid.NewGuid(), Name = "Pombo", Velocity = 60 };

        await repository.AddAsync(bird);
        await repository.SaveChangesAsync();

        var result = await repository.GetByIdAsync(bird.Id);
        Assert.NotNull(result);
        Assert.Equal(bird.Name, result!.Name);
    }

    [Fact]
    public async Task GetByIdAsync_MissingBird_ReturnsNull()
    {
        await using var context = CreateContext();
        var repository = new BirdRepository(context);

        var result = await repository.GetByIdAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public async Task GetPagedAsync_ReturnsRequestedPageAndTotalCount()
    {
        await using var context = CreateContext();
        var repository = new BirdRepository(context);
        for (var i = 0; i < 15; i++)
        {
            await repository.AddAsync(new Bird { Id = Guid.NewGuid(), Name = $"Ave {i}" });
        }
        await repository.SaveChangesAsync();

        var page = await repository.GetPagedAsync(2, 10);

        Assert.Equal(15, page.TotalCount);
        Assert.Equal(5, page.Items.Count);
        Assert.Equal(2, page.Page);
    }

    [Fact]
    public async Task Update_PersistsChanges()
    {
        await using var context = CreateContext();
        var repository = new BirdRepository(context);
        var bird = new Bird { Id = Guid.NewGuid(), Name = "Coruja" };
        await repository.AddAsync(bird);
        await repository.SaveChangesAsync();

        bird.Name = "Coruja Noturna";
        repository.Update(bird);
        await repository.SaveChangesAsync();

        var updated = await repository.GetByIdAsync(bird.Id);
        Assert.Equal("Coruja Noturna", updated!.Name);
    }

    [Fact]
    public async Task Delete_RemovesBird()
    {
        await using var context = CreateContext();
        var repository = new BirdRepository(context);
        var bird = new Bird { Id = Guid.NewGuid(), Name = "Pato" };
        await repository.AddAsync(bird);
        await repository.SaveChangesAsync();

        repository.Delete(bird);
        await repository.SaveChangesAsync();

        Assert.False(await repository.ExistsAsync(bird.Id));
    }

    [Fact]
    public async Task ExistsAsync_ReflectsPersistedState()
    {
        await using var context = CreateContext();
        var repository = new BirdRepository(context);
        var bird = new Bird { Id = Guid.NewGuid(), Name = "Avestruz" };

        Assert.False(await repository.ExistsAsync(bird.Id));

        await repository.AddAsync(bird);
        await repository.SaveChangesAsync();

        Assert.True(await repository.ExistsAsync(bird.Id));
    }
}
