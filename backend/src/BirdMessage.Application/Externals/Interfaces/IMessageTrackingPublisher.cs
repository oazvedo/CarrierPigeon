using BirdMessage.Domain.Entities;

namespace BirdMessage.Application.Externals.Interfaces;

public interface IMessageTrackingPublisher
{
    Task PublishAsync(Message message, CancellationToken cancellationToken = default);
}
