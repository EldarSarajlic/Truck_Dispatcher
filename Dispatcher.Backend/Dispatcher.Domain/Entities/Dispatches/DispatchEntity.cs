using Dispatcher.Domain.Common;
using Dispatcher.Domain.Entities.Identity;
using Dispatcher.Domain.Entities.Shipments;
using Dispatcher.Domain.Entities.Vehicles;

namespace Dispatcher.Domain.Entities.Dispatches;

/// <summary>
/// Dispatch assigns resources (truck, driver, trailer) to a shipment
/// Created by dispatcher after shipment is ready
/// </summary>
public class DispatchEntity : BaseEntity
{
    /// <summary>
    /// The shipment being dispatched (one-to-one)
    /// </summary>
    public int ShipmentId { get; set; }
    public ShipmentEntity Shipment { get; set; }

    /// <summary>
    /// Assigned truck
    /// </summary>
    public int TruckId { get; set; }
    public TruckEntity Truck { get; set; }

    /// <summary>
    /// Assigned driver
    /// </summary>
    public int DriverId { get; set; }
    public UserEntity Driver { get; set; }

    /// <summary>
    /// Assigned trailer (optional)
    /// </summary>
    public int? TrailerId { get; set; }
    public TrailerEntity? Trailer { get; set; }

    /// <summary>
    /// When this dispatch was assigned
    /// </summary>
    public DateTime AssignedAt { get; set; }

    /// <summary>
    /// Who assigned this dispatch
    /// </summary>
    public int AssignedByUserId { get; set; }
    public UserEntity AssignedBy { get; set; }

    /// <summary>
    /// Current dispatch status
    /// </summary>
    public string Status { get; set; } // "Scheduled", "InProgress", "Completed", "Cancelled"

    /// <summary>
    /// Additional notes
    /// </summary>
    public string? Notes { get; set; }

    public static class Constraints
    {
        public const int StatusMaxLength = 30;
        public const int NotesMaxLength = 1000;
    }
}