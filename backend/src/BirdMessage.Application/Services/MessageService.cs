using BirdMessage.Application.Externals.Interfaces;
using BirdMessage.Application.Mappers;
using BirdMessage.Application.Services.Interface;
using BirdMessage.Domain.Common;
using BirdMessage.Domain.Entities;
using BirdMessage.Domain.Interfaces;

namespace BirdMessage.Application.Services
{
    public class MessageService(
        IMessageRepository messageRepository,
        IAddressService addressService,
        IGeocodingService geocodingService,
        IBirdService birdService,
        IDeliveryEstimationService deliveryEstimationService,
        IMessageTrackingPublisher messageTrackingPublisher) : IMessageService
    {
        public Task<Message?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => messageRepository.GetByIdAsync(id, cancellationToken);

        public Task<PaginatedResult<Message>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
            => messageRepository.GetPagedAsync(page, pageSize, cancellationToken);

        public async Task<Message> CreateAsync(Message message, CancellationToken cancellationToken = default)
        {
            message.CreatedAt = DateTime.UtcNow;

            var bird = await birdService.GetByIdAsync(message.BirdId, cancellationToken)
                ?? throw new InvalidOperationException($"Bird '{message.BirdId}' not found.");

            var senderLatestAddress = await addressService.GetLatestByUserIdAsync(message.SenderId, cancellationToken);
            var receiverLatestAddress = await addressService.GetLatestByUserIdAsync(message.ReceiverId, cancellationToken);

            if (senderLatestAddress is not null && receiverLatestAddress is not null)
            {
                var route = await geocodingService.GetRouteAsync(
                    senderLatestAddress.ToGeocodingRoutePoint(),
                    receiverLatestAddress.ToGeocodingRoutePoint(),
                    cancellationToken);

                message.SenderLatitude = route.Origin.Latitude;
                message.SenderLongitude = route.Origin.Longitude;
                message.ReceiverLatitude = route.Destination.Latitude;
                message.ReceiverLongitude = route.Destination.Longitude;
                message.Distance = route.DistanceKm;

                var duration = deliveryEstimationService.CalculateDuration(message.Distance, bird.Velocity);
                message.EstimatedDeliveryMinutes = (decimal)duration.TotalMinutes;
            }

            await messageRepository.AddAsync(message, cancellationToken);
            await messageRepository.SaveChangesAsync(cancellationToken);

            if (message.EstimatedDeliveryMinutes > 0)
                await messageTrackingPublisher.PublishAsync(message, cancellationToken);

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
