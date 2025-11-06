namespace Dispatcher.Application.Modules.Vehicles.VehicleStatus.Commands.Delete;

public class DeleteVehicleStatusCommandHandler(IAppDbContext context,IAppCurrentUser appCurrentUser)
    : IRequestHandler<DeleteVehicleStatusCommand, Unit>
{
    public async Task<Unit> Handle(DeleteVehicleStatusCommand request, CancellationToken cancellationToken)
    {
        if (appCurrentUser.UserId is null)
            throw new MarketBusinessRuleException("123", "Korisnik nije autentifikovan.");

        var status = await context.VehicleStatuses
           .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (status is null)
            throw new MarketNotFoundException("Vehicle status not found.");

        status.IsDeleted = true; //-Soft delete
        //context.VehicleStatuses.Remove(status);
        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;          
    }
}