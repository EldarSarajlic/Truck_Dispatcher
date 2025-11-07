
namespace Dispatcher.Application.Modules.Shipments.Shipment.Querries.GetById
{
    public class GetShipmentByIdQueryDto
    {
        public int Id { get; set; }
        public decimal Weight { get; set; }
        public decimal Volume { get; set; }
        public string PickupLocation { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public DateTime? ModifiedAtUtc { get; set; }
        public bool IsDeleted { get; set; }
    }
}