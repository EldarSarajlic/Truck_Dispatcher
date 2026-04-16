using System.ComponentModel.DataAnnotations;

namespace Dispatcher.Shared.Options;

public sealed class EmailOptions
{
    public const string SectionName = "Email";

    [Required] public string Host { get; init; } = default!;
    [Range(1, 65535)] public int Port { get; init; } = 587;
    [Required] public string Username { get; init; } = default!;
    [Required] public string Password { get; init; } = default!;
    [Required] public string FromAddress { get; init; } = default!;
    [Required] public string FromName { get; init; } = default!;
}
