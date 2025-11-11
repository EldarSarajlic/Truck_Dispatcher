namespace Dispatcher.Application.Modules.Vehicles.Trucks.Queries.List
{
    public class ListTruckQueryDto
    {
        public int Id { get; set; }
        public string LicensePlateNumber { get; set; }
        public string VinNumber { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public decimal Capacity { get; set; }
        public DateTime? LastMaintenanceDate { get; set; }
        public DateTime? NextMaintenanceDate { get; set; }
        public DateTime? RegistrationExpiration { get; set; }
        public DateTime? InsuranceExpiration { get; set; }
        public string? GPSDeviceId { get; set; }
        public int? VehicleStatusId { get; set; }
        public string VehicleStatusName { get; set; }
        public int EngineCapacity { get; set; }
        public int KW { get; set; }
    }
}