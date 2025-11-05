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
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public UserRole Role { get; set; } = UserRole.Client; // Default role
    public int TokenVersion { get; set; } = 0;
    public bool IsEnabled { get; set; }
    public ICollection<RefreshTokenEntity> RefreshTokens { get; private set; } 
        = new List<RefreshTokenEntity>();
}