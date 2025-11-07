using Dispatcher.Domain.Entities.Shipments;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

public class CreateShipmentCommandHandler(IAppDbContext ctx) : IRequestHandler<CreateShipmentCommand, int>
{
  
    public async Task<int> Handle(CreateShipmentCommand request, CancellationToken cancellationToken)
    {
        var shipment = new ShipmentEntity
        {
            Weight = request.Weight,
            Volume = request.Volume,
            PickupLocation = request.PickupLocation,
            Status = request.Status,
            Description = request.Description,
            CreatedAtUtc = DateTime.UtcNow,
            IsDeleted = false
        };

        ctx.Shipments.Add(shipment);
        await ctx.SaveChangesAsync(cancellationToken);

        return shipment.Id;
    }
}
