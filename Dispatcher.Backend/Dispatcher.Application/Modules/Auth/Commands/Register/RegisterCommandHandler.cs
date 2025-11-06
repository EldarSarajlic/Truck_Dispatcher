namespace Dispatcher.Application.Modules.Auth.Commands.Register
{
    /// <summary>
    /// Handler for user registration command.
    /// Creates a new user account and returns JWT tokens for immediate authentication.
    /// </summary>
    public sealed class RegisterCommandHandler(
        IAppDbContext ctx,
        IJwtTokenService jwt,
        IPasswordHasher<UserEntity> hasher,
        TimeProvider timeProvider)
        : IRequestHandler<RegisterCommand, RegisterCommandDto>
    {
        public async Task<RegisterCommandDto> Handle(RegisterCommand request, CancellationToken ct)
        {
            // 1. Normalize and validate email uniqueness
            var normalizedEmail = request.Email.Trim().ToLowerInvariant();

            var emailExists = await ctx.Users
                .AnyAsync(x => x.Email.ToLower() == normalizedEmail, ct);

            if (emailExists)
                throw new MarketConflictException("Email address is already registered.");

            // 2. Normalize display name
            var normalizedDisplayName = request.DisplayName.Trim().ToUpperInvariant();

            // Optional: Check display name uniqueness
            var displayNameExists = await ctx.Users
                .AnyAsync(x => x.NormalizedDisplayName == normalizedDisplayName, ct);

            if (displayNameExists)
                throw new MarketConflictException("Display name is already taken.");

            // 3. Hash the password
            var user = new UserEntity
            {
                FirstName = request.FirstName.Trim(),
                LastName = request.LastName.Trim(),
                DisplayName = request.DisplayName.Trim(),
                NormalizedDisplayName = normalizedDisplayName,
                Email = normalizedEmail,
                PhoneNumber = request.PhoneNumber?.Trim(),
                DateOfBirth = request.DateOfBirth,
                Role = request.Role, // Default role for new registrations
                IsEnabled = true,
                TwoFactorEnabled = false,
                AccessFailedCount = 0,
                TokenVersion = 0,
                CreatedAtUtc = timeProvider.GetUtcNow().UtcDateTime
            };

            // Hash password after creating the user object
            user.PasswordHash = hasher.HashPassword(user, request.Password);

            // 4. Add user to database
            ctx.Users.Add(user);
            await ctx.SaveChangesAsync(ct);

            // 5. Generate JWT tokens
            var tokens = jwt.IssueTokens(user);

            // 6. Store refresh token
            ctx.RefreshTokens.Add(new RefreshTokenEntity
            {
                TokenHash = tokens.RefreshTokenHash,
                ExpiresAtUtc = tokens.RefreshTokenExpiresAtUtc,
                UserId = user.Id,
                Fingerprint = request.Fingerprint,
                CreatedAtUtc = timeProvider.GetUtcNow().UtcDateTime
            });

            await ctx.SaveChangesAsync(ct);

            // 7. Return response with tokens and user info
            return new RegisterCommandDto
            {
                AccessToken = tokens.AccessToken,
                RefreshToken = tokens.RefreshTokenRaw,
                ExpiresAtUtc = tokens.RefreshTokenExpiresAtUtc,
                UserId = user.Id,
                Email = user.Email,
                DisplayName = user.DisplayName
            };
        }
    }
}
