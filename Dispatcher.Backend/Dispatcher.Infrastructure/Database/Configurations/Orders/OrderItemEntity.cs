using Dispatcher.Domain.Common;
using Dispatcher.Domain.Entities.Inventory;
using Dispatcher.Domain.Entities.Orders;

namespace Dispatcher.Infrastructure.Database.Configurations.Orders
{
    /// <summary>
    /// Items in an order - connects Order to Inventory
    /// </summary>
    public class OrderItemEntity : BaseEntity
    {
        /// <summary>
        /// Parent order
        /// </summary>
        public int OrderId { get; set; }
        public OrderEntity Order { get; set; }

        /// <summary>
        /// Inventory item being ordered
        /// </summary>
        public int InventoryId { get; set; }
        public InventoryEntity Inventory { get; set; }

        /// <summary>
        /// Quantity ordered
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// Unit price at time of order (price snapshot)
        /// </summary>
        public decimal UnitPriceAtTime { get; set; }

        /// <summary>
        /// Total line price (Quantity * UnitPriceAtTime)
        /// </summary>
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// Total weight for this line item (calculated)
        /// </summary>
        public decimal? TotalWeight { get; set; }

        /// <summary>
        /// Total volume for this line item (calculated)
        /// </summary>
        public decimal? TotalVolume { get; set; }

        /// <summary>
        /// Notes for this specific item
        /// </summary>
        public string? Notes { get; set; }

        public static class Constraints
        {
            public const int NotesMaxLength = 500;
        }
    }
}
