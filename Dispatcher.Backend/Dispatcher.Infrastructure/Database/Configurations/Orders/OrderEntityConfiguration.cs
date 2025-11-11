using Dispatcher.Domain.Entities.Orders;
using Dispatcher.Domain.Entities.Shipments;

namespace Dispatcher.Infrastructure.Database.Configurations.Orders
{
    public class OrderEntityConfiguration : IEntityTypeConfiguration<OrderEntity>
    {
        public void Configure(EntityTypeBuilder<OrderEntity> builder)
        {
            builder.ToTable("Orders");

            builder.Property(x => x.OrderNumber)
                .IsRequired()
                .HasMaxLength(OrderEntity.Constraints.OrderNumberMaxLength);

            builder.HasIndex(x => x.OrderNumber).IsUnique();

            builder.Property(x => x.DeliveryAddress)
                .IsRequired()
                .HasMaxLength(OrderEntity.Constraints.DeliveryAddressMaxLength);

            builder.Property(x => x.DeliveryContactPerson)
                .HasMaxLength(OrderEntity.Constraints.ContactPersonMaxLength);

            builder.Property(x => x.DeliveryContactPhone)
                .HasMaxLength(OrderEntity.Constraints.ContactPhoneMaxLength);

            builder.Property(x => x.OrderDate)
                .IsRequired();

            builder.Property(x => x.Status)
                .IsRequired()
                .HasMaxLength(OrderEntity.Constraints.StatusMaxLength);

            builder.Property(x => x.Priority)
                .IsRequired()
                .HasMaxLength(OrderEntity.Constraints.PriorityMaxLength);

            builder.Property(x => x.TotalAmount)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(x => x.Currency)
                .IsRequired()
                .HasMaxLength(OrderEntity.Constraints.CurrencyMaxLength);

            builder.Property(x => x.SpecialInstructions)
                .HasMaxLength(OrderEntity.Constraints.SpecialInstructionsMaxLength);

            builder.Property(x => x.Notes)
                .HasMaxLength(OrderEntity.Constraints.NotesMaxLength);

            // Relationships
            builder.HasOne(x => x.Client)
                .WithMany()
                .HasForeignKey(x => x.ClientUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.DeliveryCity)
                .WithMany()
                .HasForeignKey(x => x.DeliveryCityId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.OrderItems)
                .WithOne(x => x.Order)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // One-to-One with Shipment
            builder.HasOne(x => x.Shipment)
            .WithOne(x => x.Order)
            .HasForeignKey<ShipmentEntity>(x => x.OrderId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

            // Indexes for performance
            builder.HasIndex(x => x.ClientUserId);
            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => x.OrderDate);
        }
    }
}