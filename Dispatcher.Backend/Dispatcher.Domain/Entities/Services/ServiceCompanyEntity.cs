using Dispatcher.Domain.Common;
using Dispatcher.Domain.Entities.Location;
using System.Collections.Generic;

namespace Dispatcher.Domain.Entities.Services
{
    public class ServiceCompanyEntity : BaseEntity
    {
        public string CompanyName { get; set; }
        public string? ContactPerson { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public DateTime? MaintenanceDate { get; set; }
        public DateTime? ContractEndDate { get; set; }
        public string? Notes { get; set; }

        public int CityId { get; set; }
        public CityEntity City { get; set; }

   
        public IReadOnlyCollection<TruckServiceAssignmentEntity> TruckServiceAssignments { get; private set; } = new List<TruckServiceAssignmentEntity>();

        public static class Constraints
        {
            public const int CompanyNameMaxLength = 100;
            public const int ContactPersonMaxLength = 50;
            public const int PhoneNumberMaxLength = 20;
            public const int EmailMaxLength = 100;
            public const int AddressMaxLength = 150;
            public const int NotesMaxLength = 250;
        }
    }
}