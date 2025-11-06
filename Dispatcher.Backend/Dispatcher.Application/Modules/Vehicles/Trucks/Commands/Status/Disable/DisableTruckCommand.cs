namespace Dispatcher.Application.Modules.Vehicles.Trucks.Commands.Status.Disable;

public sealed class DisableTruckCommand : IRequest<Unit>
{
    public required int Id { get; set; }
}
