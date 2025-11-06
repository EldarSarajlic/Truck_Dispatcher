using Dispatcher.Domain.Entities.Vehicles;

namespace Dispatcher.Application.Modules.Vehicles.Trucks.Commands.Create
{
    public class CreateTruckCommandHandler(IAppDbContext ctx) : IRequestHandler<CreateTruckCommand, int>
    {
     
        public async Task<int> Handle(CreateTruckCommand request, CancellationToken cancellationToken)
        {
            // Normalizacija i provjera da je LicensePlateNumber obavezan
            var normalizedPlate = request.LicensePlateNumber?.Trim();

            if (string.IsNullOrWhiteSpace(normalizedPlate))
                throw new ValidationException("LicensePlateNumber is required.");

            // Provjeri postoji li već truck s istom tablicom
            bool exists = await ctx.Trucks.AnyAsync(
                x => x.LicensePlateNumber == normalizedPlate, cancellationToken);

            if (exists)
                throw new Exception("Truck with this LicensePlateNumber already exists.");

            var truck = new TruckEntity
            {
                LicensePlateNumber = normalizedPlate,
                VinNumber = request.VinNumber?.Trim(),
                Make = request.Make?.Trim(),
                Model = request.Model?.Trim(),
                Year = request.Year,
                Capacity = request.Capacity,
                LastMaintenanceDate = request.LastMaintenanceDate,
                NextMaintenanceDate = request.NextMaintenanceDate,
                RegistrationExpiration = request.RegistrationExpiration,
                InsuranceExpiration = request.InsuranceExpiration,
                GPSDeviceId = request.GPSDeviceId,
                VehicleStatusId = request.VehicleStatusId,
                EngineCapacity = request.EngineCapacity,
                KW = request.KW
            };

            ctx.Trucks.Add(truck);
            await ctx.SaveChangesAsync(cancellationToken);

            // Vrati ID novog trucka (zamijeni truck.Id sa TruckId ako tvoj entity to koristi)
            return truck.Id;
        }
    }
}