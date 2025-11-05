// MarketUserEntity.cs
using Dispatcher.Domain.Common;

namespace Dispatcher.Domain.Entities.Identity;

public enum UserRole
{
    Client = 0,
    Driver = 1,
    Dispatcher = 2,
    Admin = 3
}
public sealed class UserEntity : BaseEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTime? DateOfBirth { get; set; }

    // Display Information
    public string DisplayName { get; set; }
    public string NormalizedDisplayName { get; set; }

    // Authentication & Security
    public string PasswordHash { get; set; }
    public bool TwoFactorEnabled { get; set; }
    public DateTime? LockoutEnd { get; set; }
    public int AccessFailedCount { get; set; }

    // Status & Role
    public UserRole Role { get; set; } = UserRole.Client; // Default role
    public bool IsEnabled { get; set; }
    public int TokenVersion { get; set; } = 0;

    // Foreign Keys (commented until related entities are created)
    // public int? CityId { get; set; }
    // public string? ProfilePhotoUrl { get; set; } // Reference to Photos table

    // Navigation Properties
    public ICollection<RefreshTokenEntity> RefreshTokens { get; private set; }
        = new List<RefreshTokenEntity>();

    // public CityEntity? City { get; set; }
    // public PhotoEntity? ProfilePhoto { get; set; }

    public static class Constraints
    {
        public const int FirstNameMaxLength = 100;
        public const int LastNameMaxLength = 100;
        public const int EmailMaxLength = 200;
        public const int PhoneNumberMaxLength = 20;
        public const int DisplayNameMaxLength = 150;
        public const int NormalizedDisplayNameMaxLength = 150;
    }
}