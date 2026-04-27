namespace Dispatcher.Application.Modules.Users.Commands.UploadPhoto;

public sealed class UploadUserPhotoCommand : IRequest
{
    public int UserId { get; init; }
    public PhotoFile Photo { get; init; } = default!;
}
