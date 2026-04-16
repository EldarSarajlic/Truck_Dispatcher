using Dispatcher.Application.Abstractions;
using Dispatcher.Shared.Options;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Dispatcher.Infrastructure.Common;

public sealed class EmailService(IOptions<EmailOptions> options) : IEmailService
{
    private readonly EmailOptions _opts = options.Value;

    public async Task SendPasswordResetEmailAsync(string toEmail, string resetLink, CancellationToken ct)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_opts.FromName, _opts.FromAddress));
        message.To.Add(MailboxAddress.Parse(toEmail));
        message.Subject = "Password Reset — Dispatcher System";

        var body = new BodyBuilder
        {
            HtmlBody = $"""
                <!DOCTYPE html>
                <html lang="en">
                <head><meta charset="UTF-8"><meta name="viewport" content="width=device-width, initial-scale=1.0"></head>
                <body style="font-family: Arial, sans-serif; background-color: #f4f4f4; margin: 0; padding: 0;">
                  <table width="100%" cellpadding="0" cellspacing="0" style="padding: 40px 0;">
                    <tr>
                      <td align="center">
                        <table width="600" cellpadding="0" cellspacing="0" style="background: #ffffff; border-radius: 8px; padding: 40px; box-shadow: 0 2px 8px rgba(0,0,0,0.05);">
                          <tr>
                            <td>
                              <h2 style="color: #1a1a2e; margin-bottom: 16px;">Password Reset Request</h2>
                              <p style="color: #555; line-height: 1.6;">
                                We received a request to reset your password for your Dispatcher System account.
                                This link will expire in <strong>1 hour</strong>.
                              </p>
                              <p style="text-align: center; margin: 32px 0;">
                                <a href="{resetLink}"
                                   style="background-color: #4f46e5; color: #ffffff; text-decoration: none;
                                          padding: 14px 28px; border-radius: 6px; font-size: 16px; font-weight: bold;">
                                  Reset Password
                                </a>
                              </p>
                              <p style="color: #888; font-size: 13px; line-height: 1.6;">
                                If you did not request a password reset, you can safely ignore this email.
                                Your password will remain unchanged.
                              </p>
                              <hr style="border: none; border-top: 1px solid #eee; margin: 24px 0;">
                              <p style="color: #aaa; font-size: 12px;">
                                Dispatcher System &mdash; This is an automated message, please do not reply.
                              </p>
                            </td>
                          </tr>
                        </table>
                      </td>
                    </tr>
                  </table>
                </body>
                </html>
                """
        };

        message.Body = body.ToMessageBody();

        using var client = new SmtpClient();
        await client.ConnectAsync(_opts.Host, _opts.Port, SecureSocketOptions.StartTls, ct);
        await client.AuthenticateAsync(_opts.Username, _opts.Password, ct);
        await client.SendAsync(message, ct);
        await client.DisconnectAsync(true, ct);
    }
}
