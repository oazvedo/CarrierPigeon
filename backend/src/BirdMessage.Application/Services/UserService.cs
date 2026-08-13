using BirdMessage.Application.Services.Interface;
using BirdMessage.Domain.Common;
using BirdMessage.Domain.Entities;
using BirdMessage.Domain.Interfaces;

namespace BirdMessage.Application.Services
{
    public class UserService(IUserRepository repository, IPasswordHasher passwordHasher) : IUserService
    {
        public async Task<User> CreateAsync(User user, CancellationToken cancellationToken = default)
        {
            user.Password = passwordHasher.Hash(user.Password);
            user.CreatedAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;
            await repository.AddAsync(user);
            await repository.SaveChangesAsync();
            return user;
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var user = await repository.GetByIdAsync(id, cancellationToken);
            if (user is null) return;

            repository.Delete(user);
            await repository.SaveChangesAsync(cancellationToken);
        }

        public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await repository.GetByIdAsync(id, cancellationToken);
        }

        public async Task<PaginatedResult<User>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
        {
            return await repository.GetPagedAsync(page, pageSize, cancellationToken);
        }

        public async Task UpdateAsync(User user, CancellationToken cancellationToken = default)
        {
            user.Password = passwordHasher.Hash(user.Password);
            user.UpdatedAt = DateTime.UtcNow;
            repository.Update(user);
            await repository.SaveChangesAsync();
        }
    }
}