using Dispatcher.Application.Modules.Vehicles.TruckStatuses.Queries.GetById;
using Dispatcher.Application.Modules.Vehicles.VehicleStatus.Queries.GetById;

public sealed class GetVehicleStatusByIdQueryValidator : AbstractValidator<GetVehicleStatusByIdQuery>
{
    public GetVehicleStatusByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Id must be greater than zero.");
    }
}