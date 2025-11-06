using Dispatcher.Domain.Entities.Vehicles;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Dispatcher.Application.Modules.Vehicles.Trucks.Commands.Update
{
    public class UpdateTruckCommandHandler(IAppDbContext context) : IRequestHandler<UpdateTruckCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateTruckCommand request, CancellationToken cancellationToken)
        {
            var truck = await context.Trucks
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (truck is null)
                throw new MarketNotFoundException("Truck not found.");

            truck.LicensePlateNumber = request.LicensePlateNumber.Trim();
            truck.VinNumber = request.VinNumber.Trim();
            truck.Make = request.Make.Trim();
            truck.Model = request.Model.Trim();
            truck.Year = request.Year;
            truck.Capacity = request.Capacity;
            truck.LastMaintenanceDate = request.LastMaintenanceDate;
            truck.NextMaintenanceDate = request.NextMaintenanceDate;
            truck.RegistrationExpiration = request.RegistrationExpiration;
            truck.InsuranceExpiration = request.InsuranceExpiration;
            truck.GPSDeviceId = request.GPSDeviceId;
            truck.VehicleStatusId = request.VehicleStatusId;
            truck.EngineCapacity = request.EngineCapacity;
            truck.KW = request.KW;

            await context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
