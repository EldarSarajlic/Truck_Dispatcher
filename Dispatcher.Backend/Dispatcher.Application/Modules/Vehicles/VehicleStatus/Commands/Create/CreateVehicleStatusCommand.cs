namespace Dispatcher.Application.Modules.Vehicles.VehicleStatus.Commands.Create;

public class CreateVehicleStatusCommand : IRequest<int>
{
    public required string StatusName { get; set; }
    public string? Description { get; set; }
}