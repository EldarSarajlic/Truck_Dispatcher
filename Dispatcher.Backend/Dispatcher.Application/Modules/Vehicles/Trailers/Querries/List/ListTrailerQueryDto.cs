namespace Dispatcher.Application.Modules.Vehicles.Trailers.Queries.List
{
    public sealed class ListTrailerQueryDto
    {
        public int Id { get; set; }
        public string LicensePlateNumber { get; set; } = null!;
        public string Make { get; set; } = null!;
        public string Model { get; set; } = null!;
        public int Year { get; set; }
        public string Type { get; set; } = null!;
        public decimal Length { get; set; }
        public decimal Capacity { get; set; }
        public int VehicleStatusId { get; set; }
        public string VehicleStatusName { get; set; } = null!;
        public DateTime? RegistrationExpiration { get; set; }
        public DateTime? InsuranceExpiration { get; set; }
    }
}
