using System.ComponentModel.DataAnnotations;

namespace Dispatcher.Shared.Options;

public sealed class CloudinaryOptions
{
    public const string SectionName = "Cloudinary";

    [Required] public string CloudName { get; init; } = default!;
    [Required] public string ApiKey { get; init; } = default!;
    [Required] public string ApiSecret { get; init; } = default!;
}
