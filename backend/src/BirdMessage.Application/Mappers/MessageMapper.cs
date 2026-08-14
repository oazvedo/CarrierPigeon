using BirdMessage.Application.Dto;
using BirdMessage.Domain.Common;
using BirdMessage.Domain.Entities;

namespace BirdMessage.Application.Mappers;

public static class MessageMapper
{
    public static MessageResponseDto ToResponseDto(this Message message)
        => new(
            message.Id,
            message.SenderId,
            message.ReceiverId,
            message.Text,
            message.AttachmentUrl,
            message.AttachmentType,
            message.SenderLatitude,
            message.SenderLongitude,
            message.ReceiverLatitude,
            message.ReceiverLongitude,
            message.CreatedAt);

    public static PaginatedResult<MessageResponseDto> ToResponseDto(this PaginatedResult<Message> result)
        => new(result.Items.Select(ToResponseDto).ToList(), result.TotalCount, result.Page, result.PageSize);

    public static Message ToEntity(this CreateMessageRequestDto dto)
        => new()
        {
            SenderId = dto.SenderId,
            ReceiverId = dto.ReceiverId,
            Text = dto.Text,
            AttachmentUrl = dto.AttachmentUrl,
            AttachmentType = dto.AttachmentType,
            SenderLatitude = dto.SenderLatitude,
            SenderLongitude = dto.SenderLongitude,
            ReceiverLatitude = dto.ReceiverLatitude,
            ReceiverLongitude = dto.ReceiverLongitude
        };

    public static void ApplyTo(this UpdateMessageRequestDto dto, Message message)
    {
        message.Text = dto.Text;
        message.AttachmentUrl = dto.AttachmentUrl;
        message.AttachmentType = dto.AttachmentType;
    }
}
