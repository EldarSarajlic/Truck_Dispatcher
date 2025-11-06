namespace Dispatcher.Application.Modules.Vehicles.Trucks.Commands.Delete;

public class DeleteTruckCommand : IRequest<Unit>
{
    public required int Id { get; set; }
}
