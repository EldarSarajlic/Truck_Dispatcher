namespace Dispatcher.Application.Modules.Shipments.Shipment.Querries.List
{
    public sealed class ListShipmentQueryDto
    {
        public int Id { get; init; }

        // Shipment details
        public decimal Weight { get; init; }
        public decimal Volume { get; init; }
        public string PickupLocation { get; init; }
        public string Status { get; init; }
        public string Description { get; init; }
        public DateTime ScheduledPickupDate { get; init; }
        public DateTime ScheduledDeliveryDate { get; init; }
        public string? Notes { get; init; }

        // Order details
        public int OrderId { get; init; }
        public string OrderNumber { get; init; }

        // Client details
        public int ClientId { get; init; }
        public string ClientFirstName { get; init; }
        public string ClientLastName { get; init; }
        public string ClientDisplayName { get; init; }

        // Delivery city details
        public int DeliveryCityId { get; init; }
        public string DeliveryCityName { get; init; }

        // Route details
        public int RouteId { get; init; }
        public string RouteDescription { get; init; }

        // Timestamps
        public DateTime CreatedAtUtc { get; init; }
        public DateTime? ModifiedAtUtc { get; init; }

    }
}