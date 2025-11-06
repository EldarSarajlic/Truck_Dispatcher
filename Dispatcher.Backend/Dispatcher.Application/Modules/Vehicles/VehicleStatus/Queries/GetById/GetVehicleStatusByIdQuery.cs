using Dispatcher.Application.Modules.Vehicles.VehicleStatus.Queries.GetById;

namespace Dispatcher.Application.Modules.Vehicles.TruckStatuses.Queries.GetById;

public class GetVehicleStatusByIdQuery : IRequest<GetVehicleStatusByIdQueryDto>
{
    public int Id { get; init; }


}