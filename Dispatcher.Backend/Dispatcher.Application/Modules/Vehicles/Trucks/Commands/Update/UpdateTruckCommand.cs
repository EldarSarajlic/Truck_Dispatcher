namespace Dispatcher.Application.Modules.Vehicles.Trucks.Commands.Update
{
    public class UpdateTruckCommand : IRequest<Unit>
    {
        public required int Id { get; set; }
        public required string LicensePlateNumber { get; set; }
        public required string VinNumber { get; set; }
        public required string Make { get; set; }
        public required string Model { get; set; }
        public required int Year { get; set; }
        public required decimal Capacity { get; set; }
        public DateTime? LastMaintenanceDate { get; set; }
        public DateTime? NextMaintenanceDate { get; set; }
        public DateTime? RegistrationExpiration { get; set; }
        public DateTime? InsuranceExpiration { get; set; }
        public string? GPSDeviceId { get; set; }
        public required int VehicleStatusId { get; set; }
        public required int EngineCapacity { get; set; }
        public required int KW { get; set; }
    }
}