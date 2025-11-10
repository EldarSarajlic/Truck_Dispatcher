using Dispatcher.Domain.Common;
using Dispatcher.Domain.Entities.Identity;

namespace Dispatcher.Domain.Entities.Chat
{
    /// <summary>
    /// Represents a notification in the system.
    /// </summary>
    public class NotificationEntity : BaseEntity
    {
        /// <summary>
        /// Identifier of the user who receives the notification.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Navigation property to the user.
        /// </summary>
        public UserEntity User { get; set; }

        /// <summary>
        /// Title of the notification.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Content/message of the notification.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Indicates whether the notification has been read.
        /// </summary>
        public bool IsRead { get; set; } = false;

        /// <summary>
        /// Date and time when the notification was read (if applicable).
        /// </summary>
        public DateTime? ReadAt { get; set; }

        /// <summary>
        /// Validation constraints.
        /// </summary>
        public static class Constraints
        {
            public const int TitleMaxLength = 200;
            public const int MessageMaxLength = 1000;
        }
    }
}
