namespace Dispatcher.Infrastructure.Database.Configurations.Identity;

public sealed class PasswordResetTokenEntityConfiguration : IEntityTypeConfiguration<PasswordResetTokenEntity>
{
    public void Configure(EntityTypeBuilder<PasswordResetTokenEntity> b)
    {
        b.ToTable("PasswordResetTokens");

        b.HasKey(x => x.Id);

        b.HasIndex(x => x.TokenHash)
            .IsUnique();

        b.Property(x => x.TokenHash)
            .IsRequired()
            .HasMaxLength(500);

        b.Property(x => x.ExpiresAtUtc)
            .IsRequired();

        b.Property(x => x.IsUsed)
            .HasDefaultValue(false);

        b.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
