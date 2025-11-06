using Dispatcher.Domain.Common;
using Dispatcher.Domain.Entities.Vehicles;

namespace Dispatcher.Domain.Entities.Services
{
    public class TruckServiceAssignmentEntity : BaseEntity
    {
 
        public int TruckId { get; set; }
        public TruckEntity Truck { get; set; }

        public int ServiceCompanyId { get; set; }
        public ServiceCompanyEntity ServiceCompany { get; set; }

        public DateTime AssignedDate { get; set; }
        public decimal Cost { get; set; }

        public static class Constraints
        {
            public const int CostPrecision = 18;
            public const int CostScale = 2;
        }
    }
}