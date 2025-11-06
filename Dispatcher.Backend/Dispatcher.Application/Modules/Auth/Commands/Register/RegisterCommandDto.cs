namespace Dispatcher.Application.Modules.Auth.Commands.Register
{
    /// <summary>
    /// Response DTO after successful user registration.
    /// Contains JWT tokens for immediate authentication.
    /// </summary>
    public sealed class RegisterCommandDto
    {
        /// <summary>
        /// JWT access token – used for authorized API calls.
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// Refresh token that the client stores locally and uses to obtain a new access token.
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// Expiration time of the access token in UTC format.
        /// </summary>
        public DateTime ExpiresAtUtc { get; set; }

        /// <summary>
        /// The newly created user's ID.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// User's email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// User's display name.
        /// </summary>
        public string DisplayName { get; set; }
    }
}
