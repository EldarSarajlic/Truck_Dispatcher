using Dispatcher.Domain.Entities.Vehicles;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dispatcher.Application.Modules.Vehicles.Trailers.Commands.Update
{
    public sealed class UpdateTrailerCommandHandler(IAppDbContext context)
        : IRequestHandler<UpdateTrailerCommand, Unit>
    {
        public async Task<Unit> Handle(
            UpdateTrailerCommand request,
            CancellationToken cancellationToken)
        {
            var entity = await context.Trailers
                .Where(x => x.Id == request.Id && !x.IsDeleted)
                .FirstOrDefaultAsync(cancellationToken);

            if (entity is null)
                throw new MarketNotFoundException($"Trailer with ID {request.Id} not found.");

            // Provjera duplikata tablice
            var exists = await context.Trailers.AnyAsync(
                x => x.Id != request.Id &&
                     x.LicensePlateNumber.ToLower() == request.LicensePlateNumber.ToLower() &&
                     !x.IsDeleted,
                cancellationToken);

            if (exists)
                throw new MarketConflictException("Trailer with this LicensePlateNumber already exists.");

            entity.LicensePlateNumber = request.LicensePlateNumber.Trim();
            entity.Make = request.Make.Trim();
            entity.Model = request.Model.Trim();
            entity.Type = request.Type.Trim();
            entity.Year = request.Year;
            entity.Length = request.Length;
            entity.Capacity = request.Capacity;
            entity.VehicleStatusId = request.VehicleStatusId;
            entity.RegistrationExpiration = request.RegistrationExpiration;
            entity.InsuranceExpiration = request.InsuranceExpiration;
            entity.ModifiedAtUtc = DateTime.UtcNow;

            await context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
