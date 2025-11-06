using Dispatcher.Domain.Common;
using Dispatcher.Domain.Entities.Identity;
using Dispatcher.Domain.Entities.Vehicles;

namespace Dispatcher.Domain.Entities.Media
{
    /// <summary>
    /// Represents a photo/image in the system.
    /// Can be used for user profile photos, vehicle photos, documents, etc.
    /// </summary>
    public class PhotoEntity : BaseEntity
    {
        /// <summary>
        /// Original filename when uploaded
        /// Example: "profile-pic.jpg"
        /// </summary>
        public string OriginalFileName { get; set; }

        /// <summary>
        /// Stored filename on server (usually GUID-based to avoid conflicts)
        /// Example: "a1b2c3d4-e5f6-7890-abcd-ef1234567890.jpg"
        /// </summary>
        public string StoredFileName { get; set; }

        /// <summary>
        /// File path relative to storage root
        /// Example: "photos/users/2024/11/" or "photos/vehicles/"
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Complete URL to access the photo
        /// Example: "https://yourdomain.com/uploads/photos/users/2024/11/a1b2c3d4.jpg"
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// MIME type of the file
        /// Example: "image/jpeg", "image/png", "image/webp"
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// File size in bytes
        /// Used for storage management and validation
        /// </summary>
        public long FileSizeBytes { get; set; }

        /// <summary>
        /// Image width in pixels (optional, for display optimization)
        /// </summary>
        public int? Width { get; set; }

        /// <summary>
        /// Image height in pixels (optional, for display optimization)
        /// </summary>
        public int? Height { get; set; }

        /// <summary>
        /// Thumbnail URL (optional, for performance)
        /// Smaller version of the image for list views
        /// </summary>
        public string? ThumbnailUrl { get; set; }

        /// <summary>
        /// Category/purpose of the photo
        /// Example: "ProfilePhoto", "VehiclePhoto", "DocumentScan", "License"
        /// </summary>
        public string PhotoCategory { get; set; }

        /// <summary>
        /// Alt text for accessibility
        /// </summary>
        public string? AltText { get; set; }

        /// <summary>
        /// ID of the user who uploaded this photo
        /// </summary>
        public int UploadedByUserId { get; set; }

        /// <summary>
        /// Navigation property to the user who uploaded the photo
        /// </summary>
        public UserEntity UploadedBy { get; set; }

        /// <summary>
        /// Optional: Reference to the user if this is their profile photo
        /// </summary>
        public int? ProfilePhotoForUserId { get; set; }

        /// <summary>
        /// Navigation property to user if this is their profile photo
        /// </summary>
        public UserEntity? ProfilePhotoForUser { get; set; }

        /// <summary>
        /// Validation constraints
        /// </summary>
        public static class Constraints
        {
            public const int OriginalFileNameMaxLength = 255;
            public const int StoredFileNameMaxLength = 255;
            public const int FilePathMaxLength = 500;
            public const int UrlMaxLength = 1000;
            public const int ContentTypeMaxLength = 100;
            public const int ThumbnailUrlMaxLength = 1000;
            public const int PhotoCategoryMaxLength = 50;
            public const int AltTextMaxLength = 500;

            public const long MaxFileSizeBytes = 10 * 1024 * 1024; // 10MB
        }
    }
}
