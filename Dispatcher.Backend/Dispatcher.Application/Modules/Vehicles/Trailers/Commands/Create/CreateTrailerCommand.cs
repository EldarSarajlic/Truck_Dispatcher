
namespace Dispatcher.Application.Modules.Vehicles.Trailers.Commands
{
    public class CreateTrailerCommand : IRequest<int>
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
        /// Length of the trailer in meters.
        /// </summary>
        public decimal Length { get; set; }

        /// <summary>
        /// Maximum capacity of the trailer.
        /// </summary>
        public decimal Capacity { get; set; }

        /// <summary>
        /// Trailer registration expiration date.
        /// </summary>
        public DateTime? RegistrationExpiration { get; set; }

        /// <summary>
        /// Trailer insurance expiration date.
        /// </summary>
        public DateTime? InsuranceExpiration { get; set; }

        /// <summary>
        /// Current vehicle status identifier.
        /// </summary>
        public int VehicleStatusId { get; set; }
    }
}
