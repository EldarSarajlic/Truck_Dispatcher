namespace Dispatcher.Application.Modules.Vehicles.VehicleStatus.Commands.Update;

public sealed class UpdateVehicleStatusCommand : IRequest<Unit>
{
    [JsonIgnore]
    public  int Id { get; set; }
    public  string StatusName { get; set; }
    public string? Description { get; set; }
}