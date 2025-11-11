using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using MediatR;

namespace Dispatcher.Application.Modules.Vehicles.Trucks.Queries.List
{
    public class ListTruckQueryHandler : IRequestHandler<ListTruckQuery, List<ListTruckQueryDto>>
    {
        private readonly IAppDbContext _ctx;

        public ListTruckQueryHandler(IAppDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<List<ListTruckQueryDto>> Handle(ListTruckQuery request, CancellationToken cancellationToken)
        {
            var query = _ctx.Trucks
                .Include(t => t.VehicleStatus)
                .AsQueryable();

            // Search filter
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var search = request.Search.Trim().ToLower();
                query = query.Where(t =>
                    t.LicensePlateNumber.ToLower().Contains(search) ||
                    t.VinNumber.ToLower().Contains(search) ||
                    t.Make.ToLower().Contains(search) ||
                    t.Model.ToLower().Contains(search)
                );
            }

            // Status filter (po int vrijednosti)
            if (request.Status.HasValue)
            {
                query = query.Where(t => t.VehicleStatusId == request.Status.Value);
            }

            var projectedQuery = query
                .OrderBy(t => t.LicensePlateNumber)
                .ThenBy(t => t.Make)
                .Select(t => new ListTruckQueryDto
                {
                    Id = t.Id,
                    LicensePlateNumber = t.LicensePlateNumber,
                    VinNumber = t.VinNumber,
                    Make = t.Make,
                    Model = t.Model,
                    Year = t.Year,
                    Capacity = t.Capacity,
                    LastMaintenanceDate = t.LastMaintenanceDate,
                    NextMaintenanceDate = t.NextMaintenanceDate,
                    RegistrationExpiration = t.RegistrationExpiration,
                    InsuranceExpiration = t.InsuranceExpiration,
                    GPSDeviceId = t.GPSDeviceId,
                    // Status se prikazuje kao ime iz navigacije
                    VehicleStatusName = t.VehicleStatus != null ? t.VehicleStatus.StatusName : null,
                    EngineCapacity = t.EngineCapacity,
                    KW = t.KW
                });

            return await projectedQuery.ToListAsync(cancellationToken);
        }

    }
}
