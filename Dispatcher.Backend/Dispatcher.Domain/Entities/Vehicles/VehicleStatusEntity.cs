using Dispatcher.Domain.Common;
using System.Collections.Generic;

namespace Dispatcher.Domain.Entities.Vehicles
{
    /// <summary>
    /// Represents a status that can apply to any vehicle (truck, trailer, etc.).
    /// </summary>
    public class VehicleStatusEntity : BaseEntity
    {
        /// <summary>
        /// Unique identifier for the vehicle status.
        /// </summary>
        

        /// <summary>
        /// Name of the status (e.g., Active, In Maintenance, Inactive).
        /// </summary>
        public string StatusName { get; set; }

        /// <summary>
        /// Optional description of the status.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// List of trucks with this status.
        /// </summary>
        public IReadOnlyCollection<TruckEntity> Trucks { get; private set; } = new List<TruckEntity>();

        /// <summary>
        /// Validation constraints.
        /// </summary>
        public static class Constraints
        {
            public const int StatusNameMaxLength = 50;
            public const int DescriptionMaxLength = 250;
        }
    }
}
