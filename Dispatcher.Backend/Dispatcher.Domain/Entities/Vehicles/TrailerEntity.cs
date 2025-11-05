using Dispatcher.Domain.Common;
using System;

namespace Dispatcher.Domain.Entities.Vehicles
{
    /// <summary>
    /// Represents a trailer in the system.
    /// </summary>
    public class TrailerEntity : BaseEntity
    {
        /// <summary>
        /// License plate number of the trailer.
        /// </summary>
        public string LicensePlateNumber { get; set; }

        /// <summary>
        /// Trailer manufacturer (e.g., Schmitz, Krone).
        /// </summary>
        public string Make { get; set; }

        /// <summary>
        /// Model name of the trailer.
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// Year of manufacture.
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Type of trailer (e.g., flatbed, refrigerated, tank).
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Trailer registration expiration date.
        /// </summary>
        public DateTime? RegistrationExpiration { get; set; }

        /// <summary>
        /// Trailer insurance expiration date.
        /// </summary>
        public DateTime? InsuranceExpiration { get; set; }

        /// <summary>
        /// Length of the trailer in meters.
        /// </summary>
        public decimal Length { get; set; }

        /// <summary>
        /// Maximum capacity of the trailer (tons, liters, etc.).
        /// </summary>
        public decimal Capacity { get; set; }

        /// <summary>
        /// Reference to the trailer's current vehicle status.
        /// </summary>
        public int VehicleStatusId { get; set; }

        /// <summary>
        /// Navigation property to the vehicle status.
        /// </summary>
        public VehicleStatusEntity? VehicleStatus { get; set; }

        /// <summary>
        /// Validation constraints.
        /// </summary>
        public static class Constraints
        {
            public const int LicensePlateMaxLength = 15;
            public const int MakeMaxLength = 50;
            public const int ModelMaxLength = 50;
            public const int TypeMaxLength = 50;
        }
    }
}
