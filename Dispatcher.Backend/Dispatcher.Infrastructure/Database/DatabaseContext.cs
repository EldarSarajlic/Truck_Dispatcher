using Dispatcher.Application.Abstractions;
using Dispatcher.Domain.Entities.Vehicles;

namespace Dispatcher.Infrastructure.Database;

public partial class DatabaseContext : DbContext, IAppDbContext
{
    public DbSet<MarketUserEntity> Users => Set<MarketUserEntity>();
    public DbSet<RefreshTokenEntity> RefreshTokens => Set<RefreshTokenEntity>();
    public DbSet<VehicleStatusEntity> VehicleStatuses => Set<VehicleStatusEntity>();
    public DbSet<TruckEntity> Trucks => Set<TruckEntity>();

    public DbSet<TrailerEntity> Trailers => Set<TrailerEntity>();
    private readonly TimeProvider _clock;
    public DatabaseContext(DbContextOptions<DatabaseContext> options, TimeProvider clock) : base(options)
    {
        _clock = clock;
    }
}