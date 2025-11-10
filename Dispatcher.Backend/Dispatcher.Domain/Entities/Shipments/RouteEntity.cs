using Dispatcher.Domain.Common;
using Dispatcher.Domain.Entities.Location;

namespace Dispatcher.Domain.Entities.Shipments
{
    public class RouteEntity : BaseEntity
    {
        public int StartLocationId { get; set; }
        public int EndLocationId { get; set; }
        public TimeSpan EstimatedDuration { get; set; }

        // Navigacione veze
        public CityEntity StartLocation { get; set; }
        public CityEntity EndLocation { get; set; }

        public static class Constraints
        {

        }
    }
}
