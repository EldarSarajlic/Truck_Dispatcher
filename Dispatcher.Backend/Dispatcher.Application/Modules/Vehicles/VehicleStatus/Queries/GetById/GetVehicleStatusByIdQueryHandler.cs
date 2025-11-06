using Dispatcher.Application.Modules.Vehicles.TruckStatuses.Queries.GetById;

namespace Dispatcher.Application.Modules.Vehicles.VehicleStatus.Queries.GetById;

public class GetVehicleStatusByIdQueryHandler(IAppDbContext context) : IRequestHandler<GetVehicleStatusByIdQuery, GetVehicleStatusByIdQueryDto>
{
    public async Task<GetVehicleStatusByIdQueryDto> Handle(GetVehicleStatusByIdQuery request, CancellationToken cancellationToken)
    {
        var status = await context.VehicleStatuses
            .Where(vs => vs.Id == request.Id && !vs.IsDeleted)
            .Select(vs => new GetVehicleStatusByIdQueryDto
            {
                Id = vs.Id,
                StatusName = vs.StatusName,
                Description = vs.Description,
            
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (status is null)
            throw new MarketNotFoundException($"Vehicle status with ID {request.Id} not found.");

        return status;
    }
}