using Dispatcher.Application.Modules.Vehicles.Trucks.Commands.Status.Enable;

public sealed class EnableTruckCommandHandler(IAppDbContext ctx) : IRequestHandler<EnableTruckCommand, Unit>
{
    
    public async Task<Unit> Handle(EnableTruckCommand request, CancellationToken cancellationToken)
    {
        var truck = await ctx.Trucks
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (truck == null)
            throw new MarketNotFoundException("Truck not found.");

        // Pretpostavljamo da status s ID-em 1 (ili drugi prema tvojoj aplikaciji) znači Enabled
        int enabledStatusId = 4; // Postavi prema svojoj tabeli/statusima.

        truck.VehicleStatusId = enabledStatusId;

        await ctx.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}