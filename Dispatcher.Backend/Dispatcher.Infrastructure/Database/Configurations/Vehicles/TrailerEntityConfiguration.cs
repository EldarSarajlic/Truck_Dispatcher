using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Dispatcher.Domain.Entities.Vehicles;

public class TrailerEntityConfiguration : IEntityTypeConfiguration<TrailerEntity>
{
    public void Configure(EntityTypeBuilder<TrailerEntity> builder)
    {
        builder.ToTable("Trailers");
        builder.HasKey(t => t.Id);

        builder.Property(t => t.LicensePlateNumber)
               .IsRequired()
               .HasMaxLength(TrailerEntity.Constraints.LicensePlateMaxLength);

        builder.Property(t => t.Make)
               .IsRequired()
               .HasMaxLength(TrailerEntity.Constraints.MakeMaxLength);

        builder.Property(t => t.Model)
               .IsRequired()
               .HasMaxLength(TrailerEntity.Constraints.ModelMaxLength);

        builder.Property(t => t.Type)
               .IsRequired()
               .HasMaxLength(TrailerEntity.Constraints.TypeMaxLength);

        builder.HasOne(t => t.VehicleStatus)
               .WithMany()
               .HasForeignKey(t => t.VehicleStatusId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
