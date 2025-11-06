using Dispatcher.Domain.Entities.Vehicles;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Dispatcher.Application.Modules.Vehicles.Trucks.Commands.Delete
{
    public class DeleteTruckCommandHandler(IAppDbContext context) : IRequestHandler<DeleteTruckCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteTruckCommand request, CancellationToken cancellationToken)
        {
            var truck = await context.Trucks
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (truck is null)
                throw new MarketNotFoundException("Kamion nije pronađen.");

            // Soft delete — svojstvo IsDeleted mora postojati na TruckEntity
            truck.IsDeleted = true;
            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}