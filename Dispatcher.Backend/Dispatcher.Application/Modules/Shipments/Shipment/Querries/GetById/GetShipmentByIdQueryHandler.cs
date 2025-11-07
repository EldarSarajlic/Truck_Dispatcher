using Dispatcher.Application.Modules.Shipments.Shipment.Querries.GetById;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dispatcher.Application.Modules.Shipments.Shipment.Queries.GetById
{
    public class GetShipmentByIdQueryHandler(IAppDbContext ctx) : IRequestHandler<GetShipmentByIdQuery, GetShipmentByIdQueryDto?>
    {
   

        public async Task<GetShipmentByIdQueryDto?> Handle(GetShipmentByIdQuery request, CancellationToken cancellationToken)
        {
            var shipment = await ctx.Shipments
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (shipment == null)
                return null;

            return new GetShipmentByIdQueryDto
            {
                Id = shipment.Id,
                Weight = shipment.Weight,
                Volume = shipment.Volume,
                PickupLocation = shipment.PickupLocation,
                Status = shipment.Status,
                Description = shipment.Description,
                CreatedAtUtc = shipment.CreatedAtUtc,
                ModifiedAtUtc = shipment.ModifiedAtUtc,
                IsDeleted = shipment.IsDeleted
            };
        }
    }
}