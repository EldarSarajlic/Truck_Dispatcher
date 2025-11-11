namespace Dispatcher.Application.Modules.Shipments.Shipment.Querries.List
{
    public class ListShipmentQueryDto
    {
        public int Id { get; set; }
        public decimal Weight { get; set; }
        public decimal Volume { get; set; }
        public string PickupLocation { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }

        public string? Notes { get; set; }
        // Reference for mapping 
        public int OrderId { get; set; }
        public int RouteId { get; set; } 

        // City names from routes
        public string RouteStartLocation { get; set; }
        public string RouteEndLocation { get; set; }

        public DateTime CreatedAtUtc { get; set; }
        public DateTime? ModifiedAtUtc { get; set; }
        public bool IsDeleted { get; set; }

    }
}