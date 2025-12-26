using Dispatcher.Domain.Entities.Vehicles;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dispatcher.Application.Modules.Vehicles.Trailers.Commands.Status.Change
{
    public class ChangeTrailerStatusCommandHandler(IAppDbContext ctx)
        : IRequestHandler<ChangeTrailerStatusCommand, int>
    {
        public async Task<int> Handle(
            ChangeTrailerStatusCommand request,
            CancellationToken cancellationToken)
        {
            var trailer = await ctx.Trailers
                .FirstOrDefaultAsync(x => x.Id == request.TrailerId, cancellationToken);

            if (trailer is null)
                throw new MarketNotFoundException("Trailer not found.");

            trailer.VehicleStatusId = request.VehicleStatusId;

            await ctx.SaveChangesAsync(cancellationToken);

            return trailer.Id;
        }
    }
}
