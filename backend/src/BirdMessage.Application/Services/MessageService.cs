using BirdMessage.Application.Services.Interface;
using BirdMessage.Domain.Common;
using BirdMessage.Domain.Entities;
using BirdMessage.Domain.Interfaces;

namespace BirdMessage.Application.Services
{
    public class MessageService(IMessageRepository messageRepository) : IMessageService
    {
        public Task<Message?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => messageRepository.GetByIdAsync(id, cancellationToken);

        public Task<PaginatedResult<Message>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
            => messageRepository.GetPagedAsync(page, pageSize, cancellationToken);

        public async Task<Message> CreateAsync(Message message, CancellationToken cancellationToken = default)
        {
            message.CreatedAt = DateTime.UtcNow;
            await messageRepository.AddAsync(message, cancellationToken);
            await messageRepository.SaveChangesAsync(cancellationToken);
            return message;
        }

        public async Task UpdateAsync(Message message, CancellationToken cancellationToken = default)
        {
            messageRepository.Update(message);
            await messageRepository.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var message = await messageRepository.GetByIdAsync(id, cancellationToken);
            if (message is null) return;

            messageRepository.Delete(message);
            await messageRepository.SaveChangesAsync(cancellationToken);
        }
    }
}
