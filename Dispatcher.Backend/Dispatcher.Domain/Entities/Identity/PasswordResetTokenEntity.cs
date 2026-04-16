using Dispatcher.Domain.Common;

namespace Dispatcher.Domain.Entities.Identity;

public sealed class PasswordResetTokenEntity : BaseEntity
{
    public int UserId { get; set; }
    public string TokenHash { get; set; } = default!;
    public DateTime ExpiresAtUtc { get; set; }
    public bool IsUsed { get; set; }

    // Navigation property
    public UserEntity User { get; set; } = default!;
}
