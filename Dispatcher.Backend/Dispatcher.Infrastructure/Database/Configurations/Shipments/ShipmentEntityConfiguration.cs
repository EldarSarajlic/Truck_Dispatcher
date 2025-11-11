using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Dispatcher.Domain.Entities.Shipments;

public class ShipmentEntityConfiguration : IEntityTypeConfiguration<ShipmentEntity>
{
    public void Configure(EntityTypeBuilder<ShipmentEntity> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Weight)
            .IsRequired();

        builder.Property(s => s.Volume)
            .IsRequired();

        builder.Property(s => s.PickupLocation)
            .IsRequired()
            .HasMaxLength(ShipmentEntity.Constraints.PickupLocationMaxLength);

        builder.Property(s => s.Status)
            .IsRequired()
            .HasMaxLength(ShipmentEntity.Constraints.StatusMaxLength);

        builder.Property(s => s.Description)
            .HasMaxLength(ShipmentEntity.Constraints.DescriptionMaxLength);

        builder.Property(s => s.Notes)
            .HasMaxLength(ShipmentEntity.Constraints.NotesMaxLength);

        builder.Property(s => s.ScheduledPickupDate)
            .IsRequired();

        builder.Property(s => s.ScheduledDeliveryDate)
            .IsRequired();    

         // Jedan Shipment -> jedna Order (1:1)
        builder.HasOne(s => s.Order)
            .WithOne(o => o.Shipment)
            .HasForeignKey<ShipmentEntity>(s => s.OrderId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        // Jedan Route -> više Shipments (1:N)
        builder.HasOne(s => s.Route)
            .WithMany(r => r.Shipments)
            .HasForeignKey(s => s.RouteId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        // Indeksi
        builder.HasIndex(s => s.OrderId).IsUnique();
        builder.HasIndex(s => s.RouteId);

    }
}