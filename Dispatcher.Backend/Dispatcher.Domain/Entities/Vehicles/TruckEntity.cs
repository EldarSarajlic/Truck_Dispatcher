using Dispatcher.Domain.Common;
using System;

namespace Dispatcher.Domain.Entities.Vehicles
{
    /// <summary>
    /// Represents a truck in the system.
    /// </summary>
    public class TruckEntity : BaseEntity
    {
        /// <summary>
        /// Unique identifier for the truck.
        /// </summary>
        

        /// <summary>
        /// License plate number of the truck.
        /// </summary>
        public string LicensePlateNumber { get; set; }

        /// <summary>
        /// Vehicle identification number (VIN).
        /// </summary>
        public string VinNumber { get; set; }

        /// <summary>
        /// Truck manufacturer (e.g., Volvo, Scania, MAN).
        /// </summary>
        public string Make { get; set; }

        /// <summary>
        /// Model name of the truck.
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// Year of manufacture.
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Maximum capacity of the truck (in tons, liters, etc.).
        /// </summary>
        public decimal Capacity { get; set; }

        /// <summary>
        /// Last maintenance date.
        /// </summary>
        public DateTime? LastMaintenanceDate { get; set; }

        /// <summary>
        /// Next scheduled maintenance date.
        /// </summary>
        public DateTime? NextMaintenanceDate { get; set; }

        /// <summary>
        /// Truck registration expiration date.
        /// </summary>
        public DateTime? RegistrationExpiration { get; set; }

        /// <summary>
        /// Truck insurance expiration date.
        /// </summary>
        public DateTime? InsuranceExpiration { get; set; }

        /// <summary>
        /// Identifier for the GPS device installed in the truck.
        /// </summary>
        public string? GPSDeviceId { get; set; }

        /// <summary>
        /// Reference to the truck's current vehicle status.
        /// </summary>
        public int VehicleStatusId { get; set; }

        /// <summary>
        /// Navigation property to the vehicle status.
        /// </summary>
        public VehicleStatusEntity? VehicleStatus { get; set; }

        /// <summary>
        /// Engine capacity in cubic centimeters.
        /// </summary>
        public int EngineCapacity { get; set; }

        /// <summary>
        /// Engine power in kilowatts.
        /// </summary>
        public int KW { get; set; }

        /// <summary>
        /// Validation constraints.
        /// </summary>
        public static class Constraints
        {
            public const int LicensePlateMaxLength = 15;
            public const int VinNumberMaxLength = 50;
            public const int MakeMaxLength = 50;
            public const int ModelMaxLength = 50;
        }
    }
}
