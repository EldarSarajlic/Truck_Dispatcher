using Dispatcher.Domain.Entities.Inventory;

namespace Dispatcher.Infrastructure.Database.Configurations.Inventory
{
    public class InventoryEntityConfiguration : IEntityTypeConfiguration<InventoryEntity>
    {
        public void Configure(EntityTypeBuilder<InventoryEntity> builder)
        {
            builder.ToTable("Inventory");

            builder.Property(x => x.SKU)
                .IsRequired()
                .HasMaxLength(InventoryEntity.Constraints.SKUMaxLength);

            builder.HasIndex(x => x.SKU).IsUnique();

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(InventoryEntity.Constraints.NameMaxLength);

            builder.Property(x => x.Description)
                .HasMaxLength(InventoryEntity.Constraints.DescriptionMaxLength);

            builder.Property(x => x.Category)
                .IsRequired()
                .HasMaxLength(InventoryEntity.Constraints.CategoryMaxLength);

            builder.Property(x => x.UnitOfMeasure)
                .IsRequired()
                .HasMaxLength(InventoryEntity.Constraints.UnitOfMeasureMaxLength);

            builder.Property(x => x.UnitPrice)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(x => x.UnitWeight)
                .HasPrecision(18, 3);

            builder.Property(x => x.UnitVolume)
                .HasPrecision(18, 3);

            builder.Property(x => x.IsActive)
                .HasDefaultValue(true);

            // Relationships
            builder.HasMany(x => x.OrderItems)
                .WithOne(x => x.Inventory)
                .HasForeignKey(x => x.InventoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}