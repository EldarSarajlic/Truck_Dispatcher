namespace Dispatcher.Application.Modules.Vehicles.VehicleStatus.Commands.Update;

public sealed class UpdateVehicleStatusCommandHandler(IAppDbContext context):
    IRequestHandler<UpdateVehicleStatusCommand, Unit>
{
    public async Task<Unit> Handle(UpdateVehicleStatusCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.VehicleStatuses.Where(x => x.Id == request.Id && !x.IsDeleted).FirstOrDefaultAsync(cancellationToken);

        if (entity is null)
            throw new MarketNotFoundException($"Vehicle status with ID {request.Id} not found.");

        var exists = await context.VehicleStatuses.AnyAsync(x => x.Id != request.Id && x.StatusName.ToLower() == request.StatusName.ToLower(), cancellationToken);
        if (exists)
            throw new MarketConflictException("StatusName already exists.");

        entity.StatusName = request.StatusName.Trim();
        entity.Description = request.Description;
        entity.ModifiedAtUtc = DateTime.UtcNow;

        await context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
    
}