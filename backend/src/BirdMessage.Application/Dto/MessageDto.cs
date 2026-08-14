using BirdMessage.Domain.Enums;

namespace BirdMessage.Application.Dto;

public record MessageResponseDto(
    Guid Id,
    Guid SenderId,
    Guid ReceiverId,
    string Text,
    string? AttachmentUrl,
    AttachmentType? AttachmentType,
    decimal SenderLatitude,
    decimal SenderLongitude,
    decimal ReceiverLatitude,
    decimal ReceiverLongitude,
    DateTime CreatedAt);

public record CreateMessageRequestDto(
    Guid SenderId,
    Guid ReceiverId,
    string Text,
    string? AttachmentUrl,
    AttachmentType? AttachmentType);

public record UpdateMessageRequestDto(string Text, string? AttachmentUrl, AttachmentType? AttachmentType);
