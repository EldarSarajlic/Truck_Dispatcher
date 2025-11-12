using Dispatcher.Domain.Entities.Chat;
using Dispatcher.Domain.Entities.Dispatches;
using Dispatcher.Domain.Entities.Inventory;
using Dispatcher.Domain.Entities.Media;
using Dispatcher.Domain.Entities.Orders;
using Dispatcher.Domain.Entities.Shipments;
using Dispatcher.Domain.Entities.Vehicles;

namespace Dispatcher.Application.Abstractions;

// Application layer
public interface IAppDbContext
{
    DbSet<UserEntity> Users { get; }
    DbSet<RefreshTokenEntity> RefreshTokens { get; }
    DbSet<TruckEntity> Trucks { get; }
    DbSet<VehicleStatusEntity> VehicleStatuses { get; }
    DbSet<TrailerEntity> Trailers { get; }
    DbSet<MessageEntity> Messages { get; }
    DbSet<NotificationEntity> Notifications { get; }
    DbSet<PhotoEntity> Photos { get; }
    DbSet<ShipmentEntity> Shipments { get; }
    DbSet<RouteEntity> Routes { get; }
    DbSet<InventoryEntity> Inventory { get; }
    DbSet<OrderEntity> Orders { get; }
    DbSet<OrderItemEntity> OrderItems { get; }
    DbSet<DispatchEntity> Dispatches { get; }
    Task<int> SaveChangesAsync(CancellationToken ct);
}