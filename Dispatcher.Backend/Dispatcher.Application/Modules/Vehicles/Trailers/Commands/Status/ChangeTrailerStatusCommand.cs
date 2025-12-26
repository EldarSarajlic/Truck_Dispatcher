using MediatR;

namespace Dispatcher.Application.Modules.Vehicles.Trailers.Commands.Status.Change
{
    public class ChangeTrailerStatusCommand : IRequest<int>
    {
        [JsonIgnore]
        public int TrailerId { get; set; }
        public int VehicleStatusId { get; set; }
    }
}
