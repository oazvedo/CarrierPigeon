using BirdMessage.Domain.Common;
using BirdMessage.Domain.Entities;

namespace BirdMessage.Application.Services.Interface
{
    public interface IMessageService
    {
        Task<Message?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<PaginatedResult<Message>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default);
        Task<Message> CreateAsync(Message message, CancellationToken cancellationToken = default);
        Task UpdateAsync(Message message, CancellationToken cancellationToken = default);
        Task DeleteAsync(Message message, CancellationToken cancellationToken = default);
    }
}