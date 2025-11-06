namespace Dispatcher.Application.Modules.Vehicles.VehicleStatus.Queries.List;

public sealed class ListVehicleStatusQuery : BasePagedQuery<ListVehicleStatusQueryDto>
{
    public string? Search { get; init; }
}