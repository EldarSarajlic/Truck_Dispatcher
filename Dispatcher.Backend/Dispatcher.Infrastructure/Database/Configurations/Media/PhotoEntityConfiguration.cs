using Dispatcher.Domain.Entities.Media;

namespace Dispatcher.Infrastructure.Database.Configurations.Media
{
    public class PhotoEntityConfiguration : IEntityTypeConfiguration<PhotoEntity>
    {
        public void Configure(EntityTypeBuilder<PhotoEntity> builder)
        {
            builder.ToTable("Photos");

            builder.HasKey(x => x.Id);

            // Original file information
            builder.Property(x => x.OriginalFileName)
                .IsRequired()
                .HasMaxLength(PhotoEntity.Constraints.OriginalFileNameMaxLength);

            builder.Property(x => x.StoredFileName)
                .IsRequired()
                .HasMaxLength(PhotoEntity.Constraints.StoredFileNameMaxLength);

            builder.Property(x => x.FilePath)
                .IsRequired()
                .HasMaxLength(PhotoEntity.Constraints.FilePathMaxLength);

            builder.Property(x => x.Url)
                .IsRequired()
                .HasMaxLength(PhotoEntity.Constraints.UrlMaxLength);

            // File metadata
            builder.Property(x => x.ContentType)
                .IsRequired()
                .HasMaxLength(PhotoEntity.Constraints.ContentTypeMaxLength);

            builder.Property(x => x.FileSizeBytes)
                .IsRequired();

            builder.Property(x => x.Width);
            builder.Property(x => x.Height);

            builder.Property(x => x.ThumbnailUrl)
                .HasMaxLength(PhotoEntity.Constraints.ThumbnailUrlMaxLength);

            builder.Property(x => x.PhotoCategory)
                .IsRequired()
                .HasMaxLength(PhotoEntity.Constraints.PhotoCategoryMaxLength);

            builder.Property(x => x.AltText)
                .HasMaxLength(PhotoEntity.Constraints.AltTextMaxLength);

            // Relationships

            // UploadedBy relationship (required)
            builder.HasOne(x => x.UploadedBy)
                .WithMany(x => x.UploadedPhotos)
                .HasForeignKey(x => x.UploadedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // ProfilePhotoFor relationship (optional)
            builder.HasOne(x => x.ProfilePhotoForUser)
                .WithOne(x => x.ProfilePhoto)
                .HasForeignKey<PhotoEntity>(x => x.ProfilePhotoForUserId)
                .OnDelete(DeleteBehavior.SetNull);

            // Indexes for performance
            builder.HasIndex(x => x.UploadedByUserId);
            builder.HasIndex(x => x.ProfilePhotoForUserId);
            builder.HasIndex(x => x.PhotoCategory);
            builder.HasIndex(x => x.StoredFileName).IsUnique();
        }
    }
}
