using Dispatcher.Domain.Entities.Dispatches;

namespace Dispatcher.Infrastructure.Database.Configurations.Dispatches;

public class DispatchEntityConfiguration : IEntityTypeConfiguration<DispatchEntity>
{
    public void Configure(EntityTypeBuilder<DispatchEntity> builder)
    {
        builder.ToTable("Dispatches");

        builder.Property(x => x.AssignedAt)
            .IsRequired();

        builder.Property(x => x.Status)
            .IsRequired()
            .HasMaxLength(DispatchEntity.Constraints.StatusMaxLength);

        builder.Property(x => x.Notes)
            .HasMaxLength(DispatchEntity.Constraints.NotesMaxLength);

        // Relationships
        builder.HasOne(x => x.Shipment)
            .WithOne(x => x.Dispatch)
            .HasForeignKey<DispatchEntity>(x => x.ShipmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Truck)
            .WithMany()
            .HasForeignKey(x => x.TruckId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Driver)
            .WithMany()
            .HasForeignKey(x => x.DriverId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Trailer)
            .WithMany()
            .HasForeignKey(x => x.TrailerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.AssignedBy)
            .WithMany()
            .HasForeignKey(x => x.AssignedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(x => x.ShipmentId).IsUnique();
        builder.HasIndex(x => x.DriverId);
        builder.HasIndex(x => x.TruckId);
        builder.HasIndex(x => x.Status);
    }
}