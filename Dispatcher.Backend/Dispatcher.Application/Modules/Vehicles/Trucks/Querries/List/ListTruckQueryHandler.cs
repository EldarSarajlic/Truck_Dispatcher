using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using MediatR;

namespace Dispatcher.Application.Modules.Vehicles.Trucks.Queries.List
{
    public class ListTruckQueryHandler(IAppDbContext ctx) : IRequestHandler<ListTruckQuery, List<ListTruckQueryDto>>
    {

        public async Task<List<ListTruckQueryDto>> Handle(ListTruckQuery request, CancellationToken cancellationToken)
        {
            var query = ctx.Trucks
                .Include(x => x.VehicleStatus)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                query = query.Where(x =>
                    x.LicensePlateNumber.Contains(request.Search) ||
                    x.VinNumber.Contains(request.Search) ||
                    x.Make.Contains(request.Search) ||
                    x.Model.Contains(request.Search)
                );
            }

            //if (request.OnlyEnabled)
            //{
            //    query = query.Where(x => x.VehicleStatus != null && x.VehicleStatus.IsEnabled);
            //}

            var list = await query
                .Select(x => new ListTruckQueryDto
                {
                    Id = x.Id,
                    LicensePlateNumber = x.LicensePlateNumber,
                    VinNumber = x.VinNumber,
                    Make = x.Make,
                    Model = x.Model,
                    Year = x.Year,
                    Capacity = x.Capacity,
                    LastMaintenanceDate = x.LastMaintenanceDate,
                    NextMaintenanceDate = x.NextMaintenanceDate,
                    RegistrationExpiration = x.RegistrationExpiration,
                    InsuranceExpiration = x.InsuranceExpiration,
                    GPSDeviceId = x.GPSDeviceId,
                    VehicleStatusId = x.VehicleStatusId,
                   // VehicleStatusName = x.VehicleStatus != null ? x.VehicleStatus.Name : string.Empty,
                    EngineCapacity = x.EngineCapacity,
                    KW = x.KW
                })
                .ToListAsync(cancellationToken);

            return list;
        }
    }
}