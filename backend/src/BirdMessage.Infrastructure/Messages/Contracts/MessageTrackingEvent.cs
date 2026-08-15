namespace BirdMessage.Infrastructure.Messages.Contracts
{
    public class MessageTrackingEvent
    {
        public Guid MessageId { get; init; }
        public decimal Distance { get; init; }
        public decimal EstimatedDeliveryMinutes { get; init; }
        public DateTime CreatedAt { get; init; }
    }
}
