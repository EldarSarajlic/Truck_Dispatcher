using Dispatcher.Application.Abstractions;
using Dispatcher.Domain.Entities.Chat;
using Dispatcher.Domain.Entities.Location;
using Dispatcher.Domain.Entities.Media;
using Dispatcher.Domain.Entities.Services;
using Dispatcher.Domain.Entities.Shipments;
using Dispatcher.Domain.Entities.Vehicles;

namespace Dispatcher.Infrastructure.Database;

public partial class DatabaseContext : DbContext, IAppDbContext
{
    public DbSet<UserEntity> Users => Set<UserEntity>();
    public DbSet<RefreshTokenEntity> RefreshTokens => Set<RefreshTokenEntity>();
    public DbSet<VehicleStatusEntity> VehicleStatuses => Set<VehicleStatusEntity>();
    public DbSet<TruckEntity> Trucks => Set<TruckEntity>();
    public DbSet<TrailerEntity> Trailers => Set<TrailerEntity>();
    public DbSet<CountryEntity> Country => Set<CountryEntity>();
    public DbSet<CityEntity> City => Set<CityEntity>();
    public DbSet<TruckServiceAssignmentEntity> TruckServiceAssignment => Set<TruckServiceAssignmentEntity>();
    public DbSet<ServiceCompanyEntity> ServiceCompanies => Set<ServiceCompanyEntity>();
    public DbSet<MessageEntity> Messages => Set<MessageEntity>();
    public DbSet<PhotoEntity> Photos => Set<PhotoEntity>();
    public DbSet<ShipmentEntity> Shipments => Set<ShipmentEntity>();



    private readonly TimeProvider _clock;
    public DatabaseContext(DbContextOptions<DatabaseContext> options, TimeProvider clock) : base(options)
    {
        _clock = clock;
    }
}