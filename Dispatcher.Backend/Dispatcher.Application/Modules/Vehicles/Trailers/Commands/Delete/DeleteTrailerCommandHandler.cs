using Dispatcher.Domain.Entities.Vehicles;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dispatcher.Application.Modules.Vehicles.Trailers.Commands.Delete
{
    public class DeleteTrailerCommandHandler(IAppDbContext context)
        : IRequestHandler<DeleteTrailerCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteTrailerCommand request, CancellationToken cancellationToken)
        {
            var trailer = await context.Trailers
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (trailer is null)
                throw new MarketNotFoundException("Trailer nije pronađen.");

            // Soft delete — TrailerEntity mora imati IsDeleted
            trailer.IsDeleted = true;

            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
