using Dispatcher.Domain.Entities.Media;
using Microsoft.EntityFrameworkCore;

namespace Dispatcher.Application.Modules.Users.Commands.UploadPhoto;

public sealed class UploadUserPhotoCommandHandler(
    IAppDbContext context,
    IPhotoService photoService)
    : IRequestHandler<UploadUserPhotoCommand>
{
    public async Task Handle(UploadUserPhotoCommand request, CancellationToken ct)
    {
        var user = await context.Users
            .Include(u => u.ProfilePhoto)
            .FirstOrDefaultAsync(u => u.Id == request.UserId && !u.IsDeleted, ct)
            ?? throw new MarketNotFoundException($"User with ID {request.UserId} not found.");

        // Remove old photo from Cloudinary; soft-delete DB record and sever the relationship
        if (user.ProfilePhoto is not null)
        {
            await photoService.DeleteAsync(user.ProfilePhoto.StoredFileName, ct);
            user.ProfilePhoto.ProfilePhotoForUserId = null;
            user.ProfilePhoto.IsDeleted = true;
            user.ProfilePhoto = null;
        }

        var result = await photoService.UploadAsync(
            request.Photo,
            folder: "profiles",
            photoCategory: "ProfilePhoto",
            uploadedByUserId: request.UserId,
            ct: ct);

        var photo = new PhotoEntity
        {
            OriginalFileName      = result.OriginalFileName,
            StoredFileName        = result.PublicId,
            FilePath              = result.Folder,
            Url                   = result.Url,
            ThumbnailUrl          = result.ThumbnailUrl,
            ContentType           = result.ContentType,
            FileSizeBytes         = result.FileSizeBytes,
            Width                 = result.Width,
            Height                = result.Height,
            PhotoCategory         = "ProfilePhoto",
            UploadedByUserId      = request.UserId,
            ProfilePhotoForUserId = request.UserId,
        };

        context.Photos.Add(photo);
        user.ProfilePhotoUrl = result.Url;

        await context.SaveChangesAsync(ct);
    }
}
