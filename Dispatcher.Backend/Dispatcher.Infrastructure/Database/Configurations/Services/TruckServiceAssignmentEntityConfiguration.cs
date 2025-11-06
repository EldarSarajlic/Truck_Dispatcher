using Dispatcher.Domain.Entities.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dispatcher.Infrastructure.Database.Configurations.Services
{
    public class TruckServiceAssignmentEntityConfiguration : IEntityTypeConfiguration<TruckServiceAssignmentEntity>
    {
        public void Configure(EntityTypeBuilder<TruckServiceAssignmentEntity> builder)
        {
            builder.ToTable("TruckServiceAssignments");

            builder.Property(x => x.TruckId)
                .IsRequired();

            builder.Property(x => x.ServiceCompanyId)
                .IsRequired();

            builder.Property(x => x.AssignedDate)
                .IsRequired();

            builder.Property(x => x.Cost)
                .HasPrecision(
                    TruckServiceAssignmentEntity.Constraints.CostPrecision,
                    TruckServiceAssignmentEntity.Constraints.CostScale)
                .IsRequired();

            builder.HasOne(x => x.Truck)
                .WithMany(x => x.TruckServiceAssignments)
                .HasForeignKey(x => x.TruckId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ServiceCompany)
                .WithMany(x => x.TruckServiceAssignments)
                .HasForeignKey(x => x.ServiceCompanyId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}