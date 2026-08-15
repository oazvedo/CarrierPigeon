using BirdMessage.Domain.Enums;
using BirdMessage.Domain.Interfaces;
using BirdMessage.Infrastructure.Messages.Contracts;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace BirdMessage.Infrastructure.Messages.Consumers
{
    public class MessageTrackingConsumer(IMessageRepository messageRepository, ILogger<MessageTrackingConsumer> logger) : IConsumer<MessageTrackingEvent>
    {
        private static readonly TimeSpan TrackingInterval = TimeSpan.FromSeconds(10);

        public async Task Consume(ConsumeContext<MessageTrackingEvent> context)
        {
            var trackingEvent = context.Message;
            var elapsedMinutes = (decimal)(DateTime.UtcNow - trackingEvent.CreatedAt).TotalMinutes;

            if (elapsedMinutes >= trackingEvent.EstimatedDeliveryMinutes)
            {
                var message = await messageRepository.GetByIdAsync(trackingEvent.MessageId, context.CancellationToken);
                if (message is null || message.Status == MessageStatus.Delivered)
                    return;

                message.Status = MessageStatus.Delivered;
                message.DeliveredAt = DateTime.UtcNow;
                messageRepository.Update(message);
                await messageRepository.SaveChangesAsync(context.CancellationToken);

                logger.LogInformation("Message {MessageId} delivered", trackingEvent.MessageId);
                return;
            }

            var progress = elapsedMinutes / trackingEvent.EstimatedDeliveryMinutes * 100;
            logger.LogInformation("Message {MessageId} in transit: {Progress:0.0}% complete", trackingEvent.MessageId, progress);

            await context.SchedulePublish(TrackingInterval, trackingEvent);
        }
    }
}
