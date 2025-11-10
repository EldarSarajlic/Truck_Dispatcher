using Dispatcher.Domain.Entities.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dispatcher.Infrastructure.Database.Configurations.Chat
{
    public class NotificationConfiguration : IEntityTypeConfiguration<NotificationEntity>
    {
        public void Configure(EntityTypeBuilder<NotificationEntity> builder)
        {
            builder.ToTable("Notifications");

            // Title
            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(NotificationEntity.Constraints.TitleMaxLength);

            // Message
            builder.Property(x => x.Message)
                .IsRequired()
                .HasMaxLength(NotificationEntity.Constraints.MessageMaxLength);

            // IsRead
            builder.Property(x => x.IsRead)
                .IsRequired()
                .HasDefaultValue(false);

            // ReadAt
            builder.Property(x => x.ReadAt)
                .IsRequired(false);

            // User relationship
            builder.HasOne(x => x.User)
                .WithMany() // User can have many notifications
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade); // If user is deleted, delete their notifications

            // Indexes for performance
            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => new { x.UserId, x.IsRead }); // Composite index for filtering unread notifications by user
        }
    }
}
