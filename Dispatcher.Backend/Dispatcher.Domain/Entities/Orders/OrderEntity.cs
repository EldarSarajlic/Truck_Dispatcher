using Dispatcher.Domain.Common;
using Dispatcher.Domain.Entities.Identity;
using Dispatcher.Domain.Entities.Location;
using Dispatcher.Domain.Entities.Shipments;

namespace Dispatcher.Domain.Entities.Orders
{
    /// <summary>
    /// Customer order entity
    /// </summary>
    public class OrderEntity : BaseEntity
    {
        /// <summary>
        /// Unique order number (e.g., "ORD-2024-001")
        /// </summary>
        public string OrderNumber { get; set; }

        /// <summary>
        /// Client who placed the order
        /// </summary>
        public int ClientUserId { get; set; }
        public UserEntity Client { get; set; }

        /// <summary>
        /// Delivery address specified by client
        /// </summary>
        public string DeliveryAddress { get; set; }

        /// <summary>
        /// Delivery city
        /// </summary>
        public int DeliveryCityId { get; set; }
        public CityEntity DeliveryCity { get; set; }

        /// <summary>
        /// Contact person at delivery location
        /// </summary>
        public string? DeliveryContactPerson { get; set; }

        /// <summary>
        /// Contact phone at delivery location
        /// </summary>
        public string? DeliveryContactPhone { get; set; }

        /// <summary>
        /// When the order was placed
        /// </summary>
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// Requested delivery date
        /// </summary>
        public DateTime? RequestedDeliveryDate { get; set; }

        /// <summary>
        /// Order status
        /// </summary>
        public string Status { get; set; } // "Pending", "Approved", "InProgress", "Completed", "Cancelled"

        /// <summary>
        /// Priority level
        /// </summary>
        public string Priority { get; set; } // "Low", "Normal", "High", "Urgent"

        /// <summary>
        /// Total order amount (calculated from order items)
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Currency
        /// </summary>
        public string Currency { get; set; } // "BAM", "EUR", "USD"

        /// <summary>
        /// Special instructions from client
        /// </summary>
        public string? SpecialInstructions { get; set; }

        /// <summary>
        /// Additional notes
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// Items in this order (one order can have many items)
        /// </summary>
        public IReadOnlyCollection<OrderItemEntity> OrderItems { get; private set; }
            = new List<OrderItemEntity>();

        /// <summary>
        /// One order has ONE shipment (nullable until dispatcher creates it)
        /// </summary>
        public ShipmentEntity? Shipment { get; set; }

        public static class Constraints
        {
            public const int OrderNumberMaxLength = 50;
            public const int DeliveryAddressMaxLength = 300;
            public const int ContactPersonMaxLength = 100;
            public const int ContactPhoneMaxLength = 20;
            public const int StatusMaxLength = 30;
            public const int PriorityMaxLength = 20;
            public const int CurrencyMaxLength = 10;
            public const int SpecialInstructionsMaxLength = 1000;
            public const int NotesMaxLength = 1000;
        }
    }
}
