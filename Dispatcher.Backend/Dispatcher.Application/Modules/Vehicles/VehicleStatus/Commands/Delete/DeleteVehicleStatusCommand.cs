namespace Dispatcher.Application.Modules.Vehicles.VehicleStatus.Commands.Delete;

public class DeleteVehicleStatusCommand : IRequest<Unit>
{
    public required int Id { get; set; }
}