using Dispatcher.Application.Modules.Vehicles.Trucks.Commands.Status.Disable;
using Microsoft.EntityFrameworkCore;

public sealed class DisableTruckCommandHandler(IAppDbContext ctx)
    : IRequestHandler<DisableTruckCommand, Unit>
{

    public async Task<Unit> Handle(DisableTruckCommand request, CancellationToken cancellationToken)
    {
        var truck = await ctx.Trucks
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (truck == null)
            throw new MarketNotFoundException("Truck not found.");

        // Pretpostavljamo da postoji status Disabled u bazi sa ID-em npr. 3
        int disabledStatusId = 5; // ovo postavi prema tvojoj aplikaciji

        truck.VehicleStatusId = disabledStatusId;

        await ctx.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}