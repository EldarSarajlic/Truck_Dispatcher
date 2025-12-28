using Microsoft.EntityFrameworkCore;
using MediatR;

namespace Dispatcher.Application.Modules.Vehicles.Trailers.Queries.List
{
    public sealed class ListTrailerQueryHandler(IAppDbContext ctx)
        : IRequestHandler<ListTrailerQuery, PageResult<ListTrailerQueryDto>>
    {
        public async Task<PageResult<ListTrailerQueryDto>> Handle(
            ListTrailerQuery request,
            CancellationToken cancellationToken)
        {
            var query = ctx.Trailers
                .AsNoTracking()
                .Include(t => t.VehicleStatus)
                .Where(t => !t.IsDeleted)
                .AsQueryable();

            // === Search filter ===
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var search = request.Search.Trim().ToLower();

                query = query.Where(t =>
                    t.LicensePlateNumber.ToLower().Contains(search) ||
                    t.Make.ToLower().Contains(search) ||
                    t.Model.ToLower().Contains(search) ||
                    t.Type.ToLower().Contains(search)
                );
            }

            // === Status filter ===
            if (request.Status.HasValue)
            {
                query = query.Where(t => t.VehicleStatusId == request.Status.Value);
            }

            var projectedQuery = query
                .OrderBy(t => t.LicensePlateNumber)
                .ThenBy(t => t.Make)
                .Select(t => new ListTrailerQueryDto
                {
                    Id = t.Id,
                    LicensePlateNumber = t.LicensePlateNumber,
                    Make = t.Make,
                    Model = t.Model,
                    Year = t.Year,
                    Type = t.Type,
                    Length = t.Length,
                    Capacity = t.Capacity,
                    RegistrationExpiration = t.RegistrationExpiration,
                    InsuranceExpiration = t.InsuranceExpiration,
                    VehicleStatusId = t.VehicleStatusId,
                    VehicleStatusName = t.VehicleStatus != null
                        ? t.VehicleStatus.StatusName
                        : null
                });

            return await PageResult<ListTrailerQueryDto>.FromQueryableAsync(
                projectedQuery,
                request.Paging,
                cancellationToken);
        }
    }
}
