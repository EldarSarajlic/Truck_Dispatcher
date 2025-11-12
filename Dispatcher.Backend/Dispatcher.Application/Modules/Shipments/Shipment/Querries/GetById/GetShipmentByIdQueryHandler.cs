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
    .Include(s => s.Route)
        .ThenInclude(r => r.StartLocation)
    .Include(s => s.Route)
        .ThenInclude(r => r.EndLocation)
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
                Notes = shipment.Notes,
                OrderId = (int)shipment.OrderId,
                RouteId = shipment.RouteId,
                RouteStartLocation = shipment.Route.StartLocation.Name,
                RouteEndLocation = shipment.Route.EndLocation.Name,
                CreatedAtUtc = shipment.CreatedAtUtc,
                ModifiedAtUtc = shipment.ModifiedAtUtc,
                IsDeleted = shipment.IsDeleted
            };
        }
    }
}