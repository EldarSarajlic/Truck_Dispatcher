using Dispatcher.Domain.Entities.Location;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dispatcher.Infrastructure.Database.Configurations.Location
{
    public class CountryEntityConfiguration : IEntityTypeConfiguration<CountryEntity>
    {
        public void Configure(EntityTypeBuilder<CountryEntity> builder)
        {
            builder.ToTable("Country");

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(CountryEntity.Constraints.NameMaxLength);

            builder.Property(x => x.CountryCode)
                .HasMaxLength(CountryEntity.Constraints.CountryCodeMaxLength);

            builder.Property(x => x.PhoneCode)
                .HasMaxLength(CountryEntity.Constraints.PhoneCodeMaxLength);

            builder.Property(x => x.Currency)
                .HasMaxLength(CountryEntity.Constraints.CurrencyMaxLength);

            builder.Property(x => x.TimeZone)
                .HasMaxLength(CountryEntity.Constraints.TimeZoneMaxLength);

            builder.HasMany(x => x.Cities)
                .WithOne(x => x.Country)
                .HasForeignKey(x => x.CountryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}