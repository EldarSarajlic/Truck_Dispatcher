using Dispatcher.Application.Modules.Vehicles.Trucks.Querries.GetById;


namespace Dispatcher.Application.Modules.Vehicles.Trucks.Queries.GetById
{
    public class GetTruckByIdQueryHandler(IAppDbContext context) : IRequestHandler<GetTruckByIdQuery, GetTruckByIdQueryDto?>
    {
   
        public async Task<GetTruckByIdQueryDto?> Handle(GetTruckByIdQuery request, CancellationToken cancellationToken)
        {
            var truck = await context.Trucks
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (truck == null)
                return null;

            return new GetTruckByIdQueryDto
            {
                Id = truck.Id,
                LicensePlateNumber = truck.LicensePlateNumber,
                VinNumber = truck.VinNumber,
                Make = truck.Make,
                Model = truck.Model,
                Year = truck.Year,
                Capacity = truck.Capacity,
                LastMaintenanceDate = truck.LastMaintenanceDate,
                NextMaintenanceDate = truck.NextMaintenanceDate,
                RegistrationExpiration = truck.RegistrationExpiration,
                InsuranceExpiration = truck.InsuranceExpiration,
                GPSDeviceId = truck.GPSDeviceId,
                VehicleStatusId = truck.VehicleStatusId,
                EngineCapacity = truck.EngineCapacity,
                KW = truck.KW
            };
        }
    }
}