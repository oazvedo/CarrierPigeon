using BirdMessage.Domain.Enums;

namespace BirdMessage.Domain.Entities;

public class Message
{
    public Guid Id { get; set; }
    public Guid SenderId { get; set; }
    public Guid ReceiverId { get; set; }
    public string Text { get; set; } = string.Empty;
    public string? AttachmentUrl { get; set; }
    public AttachmentType? AttachmentType { get; set; }

    // Sender/receiver coordinates captured at send time, since users can move
    // and the delivery distance/time must stay fixed to when the message was sent.
    public decimal SenderLatitude { get; set; }
    public decimal SenderLongitude { get; set; }
    public decimal ReceiverLatitude { get; set; }
    public decimal ReceiverLongitude { get; set; }
}
