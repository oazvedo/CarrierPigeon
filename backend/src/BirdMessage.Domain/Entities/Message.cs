using BirdMessage.Domain.Enums;

namespace BirdMessage.Domain.Entities;

public class Message
{
    public Guid Id { get; set; }
    public Guid SenderId { get; set; }
    public Guid ReceiverId { get; set; }
    public Guid BirdId { get; set; }
    public string Text { get; set; } = string.Empty;
    public string? AttachmentUrl { get; set; }
    public AttachmentType? AttachmentType { get; set; }
    public decimal SenderLatitude { get; set; }
    public decimal SenderLongitude { get; set; }
    public decimal ReceiverLatitude { get; set; }
    public decimal ReceiverLongitude { get; set; }
    public decimal Distance { get; set; }
    public decimal EstimatedDeliveryMinutes { get; set; }
    public MessageStatus Status { get; set; } = MessageStatus.InTransit;
    public DateTime? DeliveredAt { get; set; }
    public DateTime CreatedAt {get; set;}
}
