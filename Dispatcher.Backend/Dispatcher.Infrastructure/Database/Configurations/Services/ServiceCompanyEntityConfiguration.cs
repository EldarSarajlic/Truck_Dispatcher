using Dispatcher.Domain.Entities.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dispatcher.Infrastructure.Database.Configurations.Services
{
    public class ServiceCompanyEntityConfiguration : IEntityTypeConfiguration<ServiceCompanyEntity>
    {
        public void Configure(EntityTypeBuilder<ServiceCompanyEntity> builder)
        {
            builder.ToTable("ServiceCompanies");

            builder.Property(x => x.CompanyName)
                .HasMaxLength(ServiceCompanyEntity.Constraints.CompanyNameMaxLength)
                .IsRequired();

            builder.Property(x => x.ContactPerson)
                .HasMaxLength(ServiceCompanyEntity.Constraints.ContactPersonMaxLength);

            builder.Property(x => x.PhoneNumber)
                .HasMaxLength(ServiceCompanyEntity.Constraints.PhoneNumberMaxLength);

            builder.Property(x => x.Email)
                .HasMaxLength(ServiceCompanyEntity.Constraints.EmailMaxLength);

            builder.Property(x => x.Address)
                .HasMaxLength(ServiceCompanyEntity.Constraints.AddressMaxLength);

            builder.Property(x => x.Notes)
                .HasMaxLength(ServiceCompanyEntity.Constraints.NotesMaxLength);

            builder.Property(x => x.MaintenanceDate)
                .IsRequired(false);

            builder.Property(x => x.ContractEndDate)
                .IsRequired(false);

            builder.Property(x => x.CityId)
                .IsRequired();

            builder.HasOne(x => x.City)
                .WithMany()
                .HasForeignKey(x => x.CityId)
                .OnDelete(DeleteBehavior.Restrict);

            // Navigation to TruckServiceAssignmentEntity
            builder.HasMany(x => x.TruckServiceAssignments)
                .WithOne(x => x.ServiceCompany)
                .HasForeignKey(x => x.ServiceCompanyId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}