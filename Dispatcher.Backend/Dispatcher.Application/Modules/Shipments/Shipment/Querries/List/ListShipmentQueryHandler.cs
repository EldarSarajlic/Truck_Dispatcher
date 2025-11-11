using MediatR;
using Microsoft.EntityFrameworkCore;
using Dispatcher.Application.Modules.Shipments.Shipment.Querries.List;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Dispatcher.Application.Modules.Shipments.Shipment.Querries.List
{
    public sealed class ListShipmentQueryHandler(IAppDbContext ctx) : IRequestHandler<ListShipmentQuery, PageResult<ListShipmentQueryDto>>
    {
        public async Task<PageResult<ListShipmentQueryDto>> Handle(ListShipmentQuery request, CancellationToken cancellationToken)
        {
            var query = ctx.Shipments
               .AsNoTracking()
               .Include(x => x.Order)
                    .ThenInclude(o => o.Client)
               .Include(x => x.Order)
                    .ThenInclude(o => o.DeliveryCity)
               .Include(x => x.Route)
                    .ThenInclude(r => r.StartLocation)
               .Include(x => x.Route)
                    .ThenInclude(r => r.EndLocation)
               .AsQueryable();

            if(!string.IsNullOrEmpty(request.Status))
            {
                var statusFilter = request.Status.Trim();
                query = query.Where(x => x.Status == statusFilter);
            }

            if(!string.IsNullOrWhiteSpace(request.ClientName))
            {
                var clientNameFilter = request.ClientName.Trim().ToLower();
                query = query.Where(x =>
                    x.Order.Client.FirstName.ToLower().Contains(clientNameFilter) ||
                    x.Order.Client.LastName.ToLower().Contains(clientNameFilter) ||
                    (x.Order.Client.FirstName + " " + x.Order.Client.LastName).ToLower().Contains(clientNameFilter));
            }

            if(!string.IsNullOrWhiteSpace(request.DeliveryCityName))
            {
                var cityNameFilter = request.DeliveryCityName.Trim().ToLower();
                query = query.Where(x => x.Order.DeliveryCity.Name.ToLower() == cityNameFilter);
            }

            var projectedQuery = query
                .OrderByDescending(x => x.CreatedAtUtc)
                .Select(x => new ListShipmentQueryDto
                {
                    Id = x.Id,
                    Weight = x.Weight,
                    Volume = x.Volume,
                    PickupLocation = x.PickupLocation,
                    Status = x.Status,
                    Description = x.Description,
                    ScheduledPickupDate = x.ScheduledPickupDate,
                    ScheduledDeliveryDate = x.ScheduledDeliveryDate,
                    Notes = x.Notes,

                    OrderId = x.OrderId,
                    OrderNumber = x.Order.OrderNumber,

                    ClientId = x.Order.ClientUserId,
                    ClientFirstName = x.Order.Client.FirstName,
                    ClientLastName = x.Order.Client.LastName,
                    ClientDisplayName = x.Order.Client.DisplayName,

                    DeliveryCityId = x.Order.DeliveryCityId,
                    DeliveryCityName = x.Order.DeliveryCity.Name,

                    RouteId = x.RouteId,
                    RouteDescription = x.Route.StartLocation.Name + " → " + x.Route.EndLocation.Name,

                    CreatedAtUtc = x.CreatedAtUtc,
                    ModifiedAtUtc = x.ModifiedAtUtc
                });
            return await PageResult<ListShipmentQueryDto>.FromQueryableAsync(
            projectedQuery, 
            request.Paging, 
            cancellationToken);
        }
    }
}