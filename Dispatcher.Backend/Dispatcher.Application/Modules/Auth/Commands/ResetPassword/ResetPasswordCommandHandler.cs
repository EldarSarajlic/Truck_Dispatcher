namespace Dispatcher.Application.Modules.Auth.Commands.ResetPassword;

public sealed class ResetPasswordCommandHandler(
    IAppDbContext ctx,
    IJwtTokenService jwt,
    IPasswordHasher<UserEntity> hasher,
    TimeProvider timeProvider)
    : IRequestHandler<ResetPasswordCommand>
{
    public async Task Handle(ResetPasswordCommand request, CancellationToken ct)
    {
        // 1. Hash the incoming raw token
        var tokenHash = jwt.HashRefreshToken(request.Token);

        var nowUtc = timeProvider.GetUtcNow().UtcDateTime;

        // 2. Look up the reset token — must be unused and not expired
        var resetToken = await ctx.PasswordResetTokens
            .Include(x => x.User)
            .FirstOrDefaultAsync(x =>
                x.TokenHash == tokenHash &&
                !x.IsUsed &&
                x.ExpiresAtUtc > nowUtc, ct);

        if (resetToken is null)
            throw new MarketConflictException("Invalid or expired reset token.");

        var user = resetToken.User;

        // 3. Hash the new password
        user.PasswordHash = hasher.HashPassword(user, request.NewPassword);

        // 4. Increment TokenVersion to invalidate all existing JWTs for this user
        user.TokenVersion++;

        // 5. Mark the reset token as used
        resetToken.IsUsed = true;

        await ctx.SaveChangesAsync(ct);
    }
}
