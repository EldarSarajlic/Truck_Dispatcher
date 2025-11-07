using Dispatcher.Domain.Common;

namespace Dispatcher.Domain.Entities.Shipments
{
    public class ShipmentEntity : BaseEntity
    {
        public decimal Weight { get; set; }
        public decimal Volume { get; set; }
        public string PickupLocation { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }

        public static class Constraints
        {
            public const int PickupLocationMaxLength = 100;
            public const int StatusMaxLength = 30;
            public const int DescriptionMaxLength = 400;
        }
    }
}