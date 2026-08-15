using BirdMessage.Domain.Enums;

namespace BirdMessage.Application.Dto;

public record MessageResponseDto(
    Guid Id,
    Guid SenderId,
    Guid ReceiverId,
    Guid BirdId,
    string Text,
    string? AttachmentUrl,
    AttachmentType? AttachmentType,
    decimal SenderLatitude,
    decimal SenderLongitude,
    decimal ReceiverLatitude,
    decimal ReceiverLongitude,
    decimal Distance,
    decimal EstimatedDeliveryMinutes,
    MessageStatus Status,
    DateTime? DeliveredAt,
    DateTime CreatedAt);

public record CreateMessageRequestDto(
    Guid SenderId,
    Guid ReceiverId,
    Guid BirdId,
    string Text,
    string? AttachmentUrl,
    AttachmentType? AttachmentType);

public record UpdateMessageRequestDto(string Text, string? AttachmentUrl, AttachmentType? AttachmentType);
