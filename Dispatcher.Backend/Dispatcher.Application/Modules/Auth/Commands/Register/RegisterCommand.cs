namespace Dispatcher.Application.Modules.Auth.Commands.Register
{
    /// <summary>
    /// Command for user registration.
    /// </summary>
    public sealed class RegisterCommand : IRequest<RegisterCommandDto>
    {
        /// <summary>
        /// User's first name.
        /// </summary>
        public string FirstName { get; init; }

        /// <summary>
        /// User's last name.
        /// </summary>
        public string LastName { get; init; }

        /// <summary>
        /// User's display name (visible to others).
        /// </summary>
        public string DisplayName { get; init; }

        /// <summary>
        /// User's email address.
        /// </summary>
        public string Email { get; init; }

        /// <summary>
        /// User's password.
        /// </summary>
        public string Password { get; init; }

        /// <summary>
        /// Password confirmation (must match Password).
        /// </summary>
        public string ConfirmPassword { get; init; }

        /// <summary>
        /// User's phone number (optional).
        /// </summary>
        public string? PhoneNumber { get; init; }

        /// <summary>
        /// User's date of birth (optional).
        /// </summary>
        public DateTime DateOfBirth { get; init; }

        /// <summary>
        /// (Optional) Client "fingerprint" / device identifier for device-bound refresh tokens.
        /// </summary>
        public string? Fingerprint { get; init; }

        /// <summary>
        /// Users role (assigned by admin during registration)
        /// </summary>
        public UserRole Role { get; init; }
    }
}
