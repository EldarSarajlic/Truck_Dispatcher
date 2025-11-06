using Dispatcher.Domain.Entities.Chat;
using Dispatcher.Domain.Entities.Media;
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
    DbSet<PhotoEntity> Photos { get; }
    Task<int> SaveChangesAsync(CancellationToken ct);
}