using BirdMessage.Application.Externals.Interfaces;
using BirdMessage.Domain.Entities;
using BirdMessage.Infrastructure.Messages.Contracts;
using MassTransit;

namespace BirdMessage.Infrastructure.Messages.Publishers
{
    public class MessageTrackingPublisher(IPublishEndpoint publishEndpoint) : IMessageTrackingPublisher
    {
        public Task PublishAsync(Message message, CancellationToken cancellationToken = default)
            => publishEndpoint.Publish(new MessageTrackingEvent
            {
                MessageId = message.Id,
                Distance = message.Distance,
                EstimatedDeliveryMinutes = message.EstimatedDeliveryMinutes,
                CreatedAt = message.CreatedAt
            }, cancellationToken);
    }
}
