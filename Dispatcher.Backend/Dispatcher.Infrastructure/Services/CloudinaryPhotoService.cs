using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Dispatcher.Application.Abstractions;
using Dispatcher.Shared.Options;
using Microsoft.Extensions.Options;

namespace Dispatcher.Infrastructure.Services;

public sealed class CloudinaryPhotoService : IPhotoService
{
    private readonly Cloudinary _cloudinary;

    public CloudinaryPhotoService(IOptions<CloudinaryOptions> options)
    {
        var opts = options.Value;
        var account = new Account(opts.CloudName, opts.ApiKey, opts.ApiSecret);
        _cloudinary = new Cloudinary(account) { Api = { Secure = true } };
    }

    public async Task<PhotoUploadResult> UploadAsync(
        PhotoFile file,
        string folder,
        string photoCategory,
        int uploadedByUserId,
        CancellationToken ct = default)
    {
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, file.Content),
            Folder = folder,
            UseFilename = false,
            UniqueFilename = true,
            Overwrite = false,
            Tags = $"{photoCategory},uploader-{uploadedByUserId}"
        };

        var result = await _cloudinary.UploadAsync(uploadParams, ct);

        if (result.Error is not null)
            throw new InvalidOperationException($"Cloudinary upload failed: {result.Error.Message}");

        var thumbnailUrl = GetThumbnailUrl(result.PublicId);

        return new PhotoUploadResult(
            PublicId: result.PublicId,
            Folder: folder,
            OriginalFileName: file.FileName,
            Url: result.SecureUrl.ToString(),
            ThumbnailUrl: thumbnailUrl,
            ContentType: file.ContentType,
            FileSizeBytes: result.Bytes,
            Width: result.Width,
            Height: result.Height);
    }

    public async Task DeleteAsync(string publicId, CancellationToken ct = default)
    {
        var result = await _cloudinary.DestroyAsync(new DeletionParams(publicId));

        if (result.Error is not null)
            throw new InvalidOperationException($"Cloudinary deletion failed: {result.Error.Message}");
    }

    public string GetThumbnailUrl(string publicId, int width = 150, int height = 150)
    {
        return _cloudinary.Api.UrlImgUp
            .Transform(new Transformation().Width(width).Height(height).Crop("fill").Quality("auto").FetchFormat("auto"))
            .BuildUrl(publicId);
    }
}
