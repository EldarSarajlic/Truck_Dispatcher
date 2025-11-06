namespace Dispatcher.Application.Modules.Vehicles.Trucks.Commands.Status.Enable;

public sealed class EnableTruckCommand : IRequest<Unit>
{
    public required int Id { get; set; }
}