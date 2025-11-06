using Dispatcher.Domain.Entities.Chat;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dispatcher.Infrastructure.Database.Configurations.Chat
{
    public class MessageConfiguration : IEntityTypeConfiguration<MessageEntity>
    {
        public void Configure(EntityTypeBuilder<MessageEntity> builder)
        {
            builder.ToTable("Messages");

            // Content
            builder.Property(x => x.Content)
                .IsRequired()
                .HasMaxLength(MessageEntity.Constraints.ContentMaxLength);

            // SentAt
            builder.Property(x => x.SentAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()"); 

            // IsRead
            builder.Property(x => x.IsRead)
                .IsRequired()
                .HasDefaultValue(false);

            // Sender relationship
            builder.HasOne(x => x.Sender)
                .WithMany() 
                .HasForeignKey(x => x.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            // Receiver relationship
            builder.HasOne(x => x.Receiver)
                .WithMany() 
                .HasForeignKey(x => x.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
