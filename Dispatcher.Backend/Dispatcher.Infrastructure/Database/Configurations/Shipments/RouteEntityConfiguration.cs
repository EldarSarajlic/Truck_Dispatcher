using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Dispatcher.Domain.Entities.Shipments;


public class RouteEntityConfiguration : IEntityTypeConfiguration<RouteEntity>
{
    public void Configure(EntityTypeBuilder<RouteEntity> builder)
    {
        builder.ToTable("Routes");
        builder.HasKey(r => r.Id);

        // StartLocation relationship
        builder.HasOne(r => r.StartLocation)
               .WithMany()
               .HasForeignKey(r => r.StartLocationId)
               .OnDelete(DeleteBehavior.Restrict);

        // EndLocation relationship
        builder.HasOne(r => r.EndLocation)
               .WithMany()
               .HasForeignKey(r => r.EndLocationId)
               .OnDelete(DeleteBehavior.Restrict);

        // EstimatedDuration
        builder.Property(r => r.EstimatedDuration)
               .IsRequired();
    }
}
