using Microsoft.EntityFrameworkCore;
using MediatR;

namespace Dispatcher.Application.Modules.Vehicles.Trailers.Queries.GetById
{
    public class GetTrailerByIdQueryHandler(IAppDbContext ctx)
        : IRequestHandler<GetTrailerByIdQuery, GetTrailerByIdQueryDto?>
    {
        public async Task<GetTrailerByIdQueryDto?> Handle(
            GetTrailerByIdQuery request,
            CancellationToken cancellationToken)
        {
            return await ctx.Trailers
                .Include(t => t.VehicleStatus)
                .Where(t => !t.IsDeleted && t.Id == request.Id)
                .Select(t => new GetTrailerByIdQueryDto
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
                })
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
