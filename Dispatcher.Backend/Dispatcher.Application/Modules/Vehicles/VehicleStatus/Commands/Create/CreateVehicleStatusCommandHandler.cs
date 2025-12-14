
using Dispatcher.Application.Abstractions;
using Dispatcher.Application.Modules.Vehicles.VehicleStatus.Commands.Create;
using Dispatcher.Domain.Entities.Vehicles;
using Microsoft.EntityFrameworkCore;

namespace Dispatcher.Application.Modules.Vehicles.TruckStatuses.Commands.Create;

public class CreateVehicleStatusCommandHandler(IAppDbContext context)
    : IRequestHandler<CreateVehicleStatusCommand, int>
{
    public async Task<int> Handle(CreateVehicleStatusCommand request, CancellationToken cancellationToken)
    {
        var normalized = request.StatusName?.Trim();

        if (string.IsNullOrWhiteSpace(normalized))
            throw new ValidationException("StatusName is required.");

        // Provjera da li status već postoji
        bool exists = await context.VehicleStatuses
            .AnyAsync(x => x.StatusName == normalized, cancellationToken);

        if (exists)
        {
            throw new MarketConflictException("StatusName already exists.");
        }

        
        var entity = new VehicleStatusEntity
        {
            StatusName = normalized,
            Description = request.Description, // može biti null
            CreatedAtUtc = DateTime.UtcNow
        };

        context.VehicleStatuses.Add(entity);
        await context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
