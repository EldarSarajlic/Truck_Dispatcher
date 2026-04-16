using System.Security.Cryptography;
using System.Text;
using Dispatcher.Shared.Options;
using Microsoft.Extensions.Options;

namespace Dispatcher.Application.Modules.Auth.Commands.ForgotPassword;

public sealed class ForgotPasswordCommandHandler(
    IAppDbContext ctx,
    IJwtTokenService jwt,
    IEmailService emailService,
    IOptions<FrontendOptions> frontendOptions,
    TimeProvider timeProvider)
    : IRequestHandler<ForgotPasswordCommand>
{
    public async Task Handle(ForgotPasswordCommand request, CancellationToken ct)
    {
        var email = request.Email.Trim().ToLowerInvariant();

        // 1. Look up user — return silently if not found (prevent user enumeration)
        var user = await ctx.Users
            .FirstOrDefaultAsync(x => x.Email.ToLower() == email && x.IsEnabled && !x.IsDeleted, ct);

        if (user is null)
            return;

        var nowUtc = timeProvider.GetUtcNow().UtcDateTime;

        // 2. Invalidate any existing unused reset tokens for this user
        var existingTokens = await ctx.PasswordResetTokens
            .Where(x => x.UserId == user.Id && !x.IsUsed)
            .ToListAsync(ct);

        foreach (var token in existingTokens)
            token.IsUsed = true;

        // 3. Generate raw token: 64 random bytes, Base64URL encoded
        var rawBytes = RandomNumberGenerator.GetBytes(64);
        var rawToken = Convert.ToBase64String(rawBytes)
            .TrimEnd('=')
            .Replace('+', '-')
            .Replace('/', '_');

        // 4. Hash the raw token (SHA256, same pattern as HashRefreshToken)
        var tokenHash = jwt.HashRefreshToken(rawToken);

        // 5. Persist the reset token
        ctx.PasswordResetTokens.Add(new PasswordResetTokenEntity
        {
            UserId = user.Id,
            TokenHash = tokenHash,
            ExpiresAtUtc = nowUtc.AddHours(1),
            IsUsed = false
        });

        await ctx.SaveChangesAsync(ct);

        // 6. Build the reset link
        var resetLink = $"{frontendOptions.Value.BaseUrl}/auth/reset-password?token={rawToken}";

        // 7. Send the email
        await emailService.SendPasswordResetEmailAsync(user.Email, resetLink, ct);
    }
}
