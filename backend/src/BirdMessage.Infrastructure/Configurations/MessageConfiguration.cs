using BirdMessage.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BirdMessage.Infrastructure.Configurations;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.ToTable("messages");

        builder.Property(m => m.Id).HasColumnName("id");
        builder.Property(m => m.SenderId).HasColumnName("sender_id");
        builder.Property(m => m.ReceiverId).HasColumnName("receiver_id");
        builder.Property(m => m.Text).HasColumnName("text");
        builder.Property(m => m.AttachmentUrl).HasColumnName("attachment_url");
        builder.Property(m => m.AttachmentType).HasColumnName("attachment_type");

        builder.Property(m => m.SenderLatitude).HasColumnName("sender_latitude").HasPrecision(9, 6);
        builder.Property(m => m.SenderLongitude).HasColumnName("sender_longitude").HasPrecision(9, 6);
        builder.Property(m => m.ReceiverLatitude).HasColumnName("receiver_latitude").HasPrecision(9, 6);
        builder.Property(m => m.ReceiverLongitude).HasColumnName("receiver_longitude").HasPrecision(9, 6);
        builder.Property(m => m.CreatedAt).HasColumnName("created_at");
    }
}
