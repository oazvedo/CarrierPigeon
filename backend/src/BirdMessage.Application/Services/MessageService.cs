using BirdMessage.Application.Services.Interface;
using BirdMessage.Domain.Common;
using BirdMessage.Domain.Entities;

namespace BirdMessage.Application.Services
{
    public class MessageService : IMessageService
    {
        public async Task<Message> CreateAsync(Message message, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Message message, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Message?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<PaginatedResult<Message>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Message message, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}