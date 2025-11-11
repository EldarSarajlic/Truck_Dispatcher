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

        // Unique index to enforce one-to-one
        builder.Property(x => x.OrderId)
           .IsRequired();

        builder.HasIndex(x => x.OrderId).IsUnique();

        builder.HasOne(x => x.Order)
            .WithOne(x => x.Shipment)
            .HasForeignKey<ShipmentEntity>(x => x.OrderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Order)
            .WithOne(x => x.Shipment)
            .HasForeignKey<ShipmentEntity>(x => x.OrderId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();  

    }
}