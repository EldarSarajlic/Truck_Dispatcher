namespace Dispatcher.Application.Abstractions;

/// <summary>
/// Represents the currently logged-in user in the system.
/// </summary>

public interface IAppCurrentUser
{
    /// <summary>
    /// User identifier (UserId).
    /// </summary>
    int? UserId { get; }

    /// <summary>
    /// User Email.
    /// </summary>
    string? Email { get; }

    /// <summary>
    /// User's first name.
    /// </summary>
    string? FirstName { get; }

    /// <summary>
    /// User's last name.
    /// </summary>
    string? LastName { get; }

    /// <summary>
    /// User's display name.
    /// </summary>
    string? DisplayName { get; }

    /// <summary>
    /// User's phone number.
    /// </summary>
    string? PhoneNumber { get; }

    /// <summary>
    /// Indicates whether the user is logged in.
    /// </summary>
    bool IsAuthenticated { get; }

    /// <summary>
    /// User's role as enum.
    /// </summary>
    UserRole? Role { get; }
}