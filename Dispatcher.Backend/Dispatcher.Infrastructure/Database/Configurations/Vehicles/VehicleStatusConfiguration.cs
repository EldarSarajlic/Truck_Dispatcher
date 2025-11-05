using Dispatcher.Domain.Entities.Vehicles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dispatcher.Infrastructure.Database.Configurations.Vehicles
{
    public class VehicleStatusConfiguration : IEntityTypeConfiguration<VehicleStatusEntity>
    {
        public void Configure(EntityTypeBuilder<VehicleStatusEntity> builder)
        {
            builder.ToTable("VehicleStatuses");

            builder.Property(x => x.StatusName)
                .IsRequired()
                .HasMaxLength(VehicleStatusEntity.Constraints.StatusNameMaxLength);

            builder.Property(x => x.Description)
                .HasMaxLength(VehicleStatusEntity.Constraints.DescriptionMaxLength);
        }
    }
}