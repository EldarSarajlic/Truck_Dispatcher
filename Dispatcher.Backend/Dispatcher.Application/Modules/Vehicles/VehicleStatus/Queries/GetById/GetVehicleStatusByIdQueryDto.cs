namespace Dispatcher.Application.Modules.Vehicles.VehicleStatus.Queries.GetById;

public class GetVehicleStatusByIdQueryDto
{

    public required int Id { get; init; }
     public required string StatusName { get; init; }

    public required string? Description { get; init; }



}