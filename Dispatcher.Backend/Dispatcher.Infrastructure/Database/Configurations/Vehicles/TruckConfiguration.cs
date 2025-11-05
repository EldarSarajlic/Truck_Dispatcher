using Dispatcher.Domain.Entities.Vehicles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dispatcher.Infrastructure.Database.Configurations.Vehicles
{
    public class TruckConfiguration : IEntityTypeConfiguration<TruckEntity>
    {
        public void Configure(EntityTypeBuilder<TruckEntity> builder)
        {
            builder.ToTable("Trucks");

            builder.Property(x => x.LicensePlateNumber)
                .IsRequired()
                .HasMaxLength(TruckEntity.Constraints.LicensePlateMaxLength);

            builder.Property(x => x.VinNumber)
                .IsRequired()
                .HasMaxLength(TruckEntity.Constraints.VinNumberMaxLength);

            builder.Property(x => x.Make)
                .IsRequired()
                .HasMaxLength(TruckEntity.Constraints.MakeMaxLength);

            builder.Property(x => x.Model)
                .IsRequired()
                .HasMaxLength(TruckEntity.Constraints.ModelMaxLength);

            builder.Property(x => x.Capacity)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(x => x.EngineCapacity)
                .IsRequired();

            builder.Property(x => x.KW)
                .IsRequired();

            builder.HasOne(x => x.VehicleStatus)
                .WithMany(x => x.Trucks)
                .HasForeignKey(x => x.VehicleStatusId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
