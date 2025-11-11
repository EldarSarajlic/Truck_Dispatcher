using MediatR;
using Microsoft.EntityFrameworkCore;
using Dispatcher.Application.Modules.Shipments.Shipment.Querries.List;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Dispatcher.Application.Modules.Shipments.Shipment.Querries.List
{
    public class ListShipmentQueryHandler : IRequestHandler<ListShipmentQuery, List<ListShipmentQueryDto>>
    {
        private readonly IAppDbContext _context;

        public ListShipmentQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ListShipmentQueryDto>> Handle(ListShipmentQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Shipments
                .AsNoTracking();

            // Filter primjeri
            if (!string.IsNullOrWhiteSpace(request.Status))
                query = query.Where(x => x.Status == request.Status);

            if (!string.IsNullOrWhiteSpace(request.PickupLocation))
                query = query.Where(x => x.PickupLocation == request.PickupLocation);

            query=query.Include(s=> s.Route).ThenInclude(r=> r.StartLocation)
                       .Include(s=> s.Route).ThenInclude(r=> r.EndLocation);

            var result = await query
                .Select(x => new ListShipmentQueryDto
                {
                    Id = x.Id,
                    Weight = x.Weight,
                    Volume = x.Volume,
                    PickupLocation = x.PickupLocation,
                    Status = x.Status,
                    Description = x.Description,
                    Notes = x.Notes,
                    OrderId = x.OrderId,
                    RouteId = x.RouteId,
                    RouteStartLocation = x.Route.StartLocation.Name,
                    RouteEndLocation = x.Route.EndLocation.Name,
                    CreatedAtUtc = x.CreatedAtUtc,
                    ModifiedAtUtc = x.ModifiedAtUtc,
                    IsDeleted = x.IsDeleted
                })
                .ToListAsync(cancellationToken);

            return result;
        }
    }
}