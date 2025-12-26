using MediatR;
using System.Text.Json.Serialization;

namespace Dispatcher.Application.Modules.Vehicles.Trailers.Commands.Update
{
    public class UpdateTrailerCommand : IRequest<Unit>
    {
        [JsonIgnore]
        public int Id { get; set; }

        public required string LicensePlateNumber { get; set; }
        public required string Make { get; set; }
        public required string Model { get; set; }
        public required int Year { get; set; }
        public required string Type { get; set; }
        public required decimal Length { get; set; }
        public required decimal Capacity { get; set; }
        public required int VehicleStatusId { get; set; }
        public DateTime? RegistrationExpiration { get; set; }
        public DateTime? InsuranceExpiration { get; set; }
    }
}
