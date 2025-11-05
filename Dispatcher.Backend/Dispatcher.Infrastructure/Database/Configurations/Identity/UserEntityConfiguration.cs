namespace Dispatcher.Infrastructure.Database.Configurations.Identity;

public sealed class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> b)
    {
        b.ToTable("Users");

        b.HasKey(x => x.Id);

        // Unique indexes
        b.HasIndex(x => x.Email)
            .IsUnique();

        b.HasIndex(x => x.NormalizedDisplayName);

        // Basic Information
        b.Property(x => x.FirstName)
            .IsRequired()
            .HasMaxLength(UserEntity.Constraints.FirstNameMaxLength);

        b.Property(x => x.LastName)
            .IsRequired()
            .HasMaxLength(UserEntity.Constraints.LastNameMaxLength);

        b.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(UserEntity.Constraints.EmailMaxLength);

        b.Property(x => x.PhoneNumber)
            .HasMaxLength(UserEntity.Constraints.PhoneNumberMaxLength);

        b.Property(x => x.DateOfBirth);

        // Display Information
        b.Property(x => x.DisplayName)
            .IsRequired()
            .HasMaxLength(UserEntity.Constraints.DisplayNameMaxLength);

        b.Property(x => x.NormalizedDisplayName)
            .IsRequired()
            .HasMaxLength(UserEntity.Constraints.NormalizedDisplayNameMaxLength);

        // Authentication & Security
        b.Property(x => x.PasswordHash)
            .IsRequired();

        b.Property(x => x.TwoFactorEnabled)
            .HasDefaultValue(false);

        b.Property(x => x.LockoutEnd);

        b.Property(x => x.AccessFailedCount)
            .HasDefaultValue(0);

        // Status & Role
        b.Property(x => x.Role)
            .HasDefaultValue(UserRole.Client);

        b.Property(x => x.TokenVersion)
            .HasDefaultValue(0);

        b.Property(x => x.IsEnabled)
            .HasDefaultValue(true);

        // Foreign Keys (commented until related entities are created)
        // b.Property(x => x.CityId);
        // b.Property(x => x.ProfilePhotoUrl);

        // Navigation
        b.HasMany(x => x.RefreshTokens)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId);

        // Relationships (commented until related entities are created)
        // b.HasOne(x => x.City)
        //     .WithMany()
        //     .HasForeignKey(x => x.CityId)
        //     .OnDelete(DeleteBehavior.SetNull);

        // b.HasOne(x => x.ProfilePhoto)
        //     .WithMany()
        //     .HasForeignKey(x => x.ProfilePhotoUrl)
        //     .OnDelete(DeleteBehavior.SetNull);
    }
}