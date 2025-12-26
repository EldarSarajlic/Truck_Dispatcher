using Dispatcher.Domain.Entities.Vehicles;
using Microsoft.EntityFrameworkCore;

namespace Dispatcher.Application.Modules.Vehicles.Trailers.Commands.Create
{
    public class CreateTrailerCommandHandler(IAppDbContext ctx)
        : IRequestHandler<CreateTrailerCommand, int>
    {
        public async Task<int> Handle(CreateTrailerCommand request, CancellationToken cancellationToken)
        {
            // Normalizacija i provjera da je LicensePlateNumber obavezan
            var normalizedPlate = request.LicensePlateNumber?.Trim();

            if (string.IsNullOrWhiteSpace(normalizedPlate))
                throw new ValidationException("LicensePlateNumber is required.");

            // Provjera da li već postoji trailer sa istom tablicom
            bool exists = await ctx.Trailers.AnyAsync(
                x => x.LicensePlateNumber == normalizedPlate,
                cancellationToken
            );

            if (exists)
                throw new Exception("Trailer with this LicensePlateNumber already exists.");

            var trailer = new TrailerEntity
            {
                LicensePlateNumber = normalizedPlate,
                Make = request.Make?.Trim(),
                Model = request.Model?.Trim(),
                Year = request.Year,
                Type = request.Type?.Trim(),
                Length = request.Length,
                Capacity = request.Capacity,
                RegistrationExpiration = request.RegistrationExpiration,
                InsuranceExpiration = request.InsuranceExpiration,
                VehicleStatusId = request.VehicleStatusId
            };

            ctx.Trailers.Add(trailer);
            await ctx.SaveChangesAsync(cancellationToken);

            return trailer.Id;
        }
    }
}
