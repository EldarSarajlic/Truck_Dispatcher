using Dispatcher.Domain.Entities.Location;

namespace Dispatcher.Infrastructure.Database.Configurations.Location
{
    public class CityEntityConfiguration : IEntityTypeConfiguration<CityEntity>
    {
        public void Configure(EntityTypeBuilder<CityEntity> builder)
        {
            builder.ToTable("City");

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(CityEntity.Constraints.NameMaxLength);

            builder.Property(x => x.PostalCode)
                .HasMaxLength(CityEntity.Constraints.PostalCodeMaxLength);

            builder.Property(x => x.CountryId)
                .IsRequired();

            builder.HasOne(x => x.Country)
                .WithMany(x => x.Cities)
                .HasForeignKey(x => x.CountryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}