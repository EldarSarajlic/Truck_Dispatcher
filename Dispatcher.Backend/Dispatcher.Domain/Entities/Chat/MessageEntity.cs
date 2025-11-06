using Dispatcher.Domain.Common;
using Dispatcher.Domain.Entities.Identity;
using System;
using System.Collections.Generic;

namespace Dispatcher.Domain.Entities.Chat
{
    /// <summary>
    /// Represents a chat message in the system.
    /// </summary>
    public class MessageEntity : BaseEntity
    {
        /// <summary>
        /// Identifier of the user who sent the message.
        /// </summary>
        public int SenderId { get; set; }

        /// <summary>
        /// Navigation property to the sender user.
        /// </summary>
        public UserEntity? Sender { get; set; }

        /// <summary>
        /// Identifier of the user who receives the message.
        /// </summary>
        public int ReceiverId { get; set; }

        /// <summary>
        /// Navigation property to the receiver user.
        /// </summary>
        public UserEntity? Receiver { get; set; }

        /// <summary>
        /// Content of the message.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Date and time when the message was sent.
        /// </summary>
        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Indicates whether the message has been read.
        /// </summary>
        public bool IsRead { get; set; } = false;

        /// <summary>
        /// Optional: Validation constraints.
        /// </summary>
        public static class Constraints
        {
            public const int ContentMaxLength = 2000;
        }
    }
}
