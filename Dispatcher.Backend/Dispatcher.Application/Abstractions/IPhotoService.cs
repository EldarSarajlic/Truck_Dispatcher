namespace Dispatcher.Application.Abstractions;

public record PhotoFile(Stream Content, string FileName, string ContentType, long Length);

public record PhotoUploadResult(
    string PublicId,
    string Folder,
    string OriginalFileName,
    string Url,
    string ThumbnailUrl,
    string ContentType,
    long FileSizeBytes,
    int? Width,
    int? Height);

public interface IPhotoService
{
    Task<PhotoUploadResult> UploadAsync(
        PhotoFile file,
        string folder,
        string photoCategory,
        int uploadedByUserId,
        CancellationToken ct = default);

    Task DeleteAsync(string publicId, CancellationToken ct = default);

    string GetThumbnailUrl(string publicId, int width = 150, int height = 150);
}
