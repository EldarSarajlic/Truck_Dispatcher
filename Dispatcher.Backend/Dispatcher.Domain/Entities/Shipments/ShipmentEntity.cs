using Dispatcher.Domain.Common;
using Dispatcher.Domain.Entities.Orders;

namespace Dispatcher.Domain.Entities.Shipments
{
    public class ShipmentEntity : BaseEntity
    {

        public int OrderId { get; set; }
        public OrderEntity Order { get; set; }

        public int RouteId { get; set; }
        public RouteEntity Route { get; set; }
        public DateTime ScheduledPickupDate { get; set; }
        public DateTime ScheduledDeliveryDate { get; set; }
        public decimal Weight { get; set; }
        public decimal Volume { get; set; }
        public string PickupLocation { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }

        public string? Notes { get; set; }

        //public IReadOnlyCollection<DispatchEntity> Dispatches { get; private set; } = new List<DispatchEntity>();
        public static class Constraints
        {
            public const int PickupLocationMaxLength = 100;
            public const int StatusMaxLength = 30;
            public const int DescriptionMaxLength = 400;
            public const int NotesMaxLength = 1000;
        }
    }
}