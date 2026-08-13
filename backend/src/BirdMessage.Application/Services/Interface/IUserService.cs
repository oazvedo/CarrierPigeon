using BirdMessage.Domain.Common;
using BirdMessage.Domain.Entities;

namespace BirdMessage.Application.Services.Interface
{
    public interface IUserService
    {
        Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<PaginatedResult<User>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default);
        Task<User> CreateAsync(User user, CancellationToken cancellationToken = default);
        Task UpdateAsync(User user, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}