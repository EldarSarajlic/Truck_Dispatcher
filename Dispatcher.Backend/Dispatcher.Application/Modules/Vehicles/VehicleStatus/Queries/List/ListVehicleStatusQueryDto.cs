namespace Dispatcher.Application.Modules.Vehicles.VehicleStatus.Queries.List;

public sealed class ListVehicleStatusQueryDto
{
    public required int Id { get; init; }
    public required string StatusName { get; init; }
    public required string? Description { get; init; }
}