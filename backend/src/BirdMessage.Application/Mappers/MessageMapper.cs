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
            message.BirdId,
            message.Text,
            message.AttachmentUrl,
            message.AttachmentType,
            message.SenderLatitude,
            message.SenderLongitude,
            message.ReceiverLatitude,
            message.ReceiverLongitude,
            message.Distance,
            message.EstimatedDeliveryMinutes,
            message.CreatedAt);

    public static PaginatedResult<MessageResponseDto> ToResponseDto(this PaginatedResult<Message> result)
        => new(result.Items.Select(ToResponseDto).ToList(), result.TotalCount, result.Page, result.PageSize);

    public static Message ToEntity(this CreateMessageRequestDto dto)
        => new()
        {
            SenderId = dto.SenderId,
            ReceiverId = dto.ReceiverId,
            BirdId = dto.BirdId,
            Text = dto.Text,
            AttachmentUrl = dto.AttachmentUrl,
            AttachmentType = dto.AttachmentType
        };

    public static void ApplyTo(this UpdateMessageRequestDto dto, Message message)
    {
        message.Text = dto.Text;
        message.AttachmentUrl = dto.AttachmentUrl;
        message.AttachmentType = dto.AttachmentType;
    }
}
