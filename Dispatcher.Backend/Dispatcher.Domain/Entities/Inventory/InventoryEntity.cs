using Dispatcher.Domain.Common;
using Dispatcher.Domain.Entities.Identity;
using Dispatcher.Domain.Entities.Orders;

namespace Dispatcher.Domain.Entities.Inventory
{
    /// <summary>
    /// Master inventory catalog - all available items that can be ordered
    /// </summary>
    public class InventoryEntity : BaseEntity
    {
        /// <summary>
        /// Unique stock keeping unit identifier
        /// </summary>
        public string SKU { get; set; }

        /// <summary>
        /// Item name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Detailed description
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Category (e.g., "Electronics", "Food", "Automotive")
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Unit of measurement (e.g., "kg", "pcs", "liters")
        /// </summary>
        public string UnitOfMeasure { get; set; }

        /// <summary>
        /// Price per unit
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Weight per unit in kg (optional)
        /// </summary>
        public decimal? UnitWeight { get; set; }

        /// <summary>
        /// Volume per unit in m³ (optional)
        /// </summary>
        public decimal? UnitVolume { get; set; }

        /// <summary>
        /// Is this item currently available for ordering
        /// </summary>
        public bool IsActive { get; set; }

        // Navigation
        public IReadOnlyCollection<OrderItemEntity> OrderItems { get; private set; }
            = new List<OrderItemEntity>();

        public static class Constraints
        {
            public const int SKUMaxLength = 50;
            public const int NameMaxLength = 200;
            public const int DescriptionMaxLength = 1000;
            public const int CategoryMaxLength = 100;
            public const int UnitOfMeasureMaxLength = 20;
        }
    }
}
