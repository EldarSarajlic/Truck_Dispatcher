using Dispatcher.Domain.Entities.Vehicles;
using FluentValidation;

namespace Dispatcher.Application.Modules.Vehicles.Trailers.Commands.Create
{
    public class CreateTrailerCommandValidator : AbstractValidator<CreateTrailerCommand>
    {
        public CreateTrailerCommandValidator()
        {
            RuleFor(x => x.LicensePlateNumber)
                .NotEmpty().WithMessage("License plate number is required.")
                .MaximumLength(TrailerEntity.Constraints.LicensePlateMaxLength);

            RuleFor(x => x.Make)
                .NotEmpty().WithMessage("Make is required.")
                .MaximumLength(TrailerEntity.Constraints.MakeMaxLength);

            RuleFor(x => x.Model)
                .NotEmpty().WithMessage("Model is required.")
                .MaximumLength(TrailerEntity.Constraints.ModelMaxLength);

            RuleFor(x => x.Type)
                .NotEmpty().WithMessage("Trailer type is required.")
                .MaximumLength(TrailerEntity.Constraints.TypeMaxLength);

            RuleFor(x => x.Year)
                .GreaterThan(1980)
                .WithMessage("Year must be greater than 1980.");

            RuleFor(x => x.Length)
                .GreaterThan(0)
                .WithMessage("Length must be greater than 0.");

            RuleFor(x => x.Capacity)
                .GreaterThan(0)
                .WithMessage("Capacity must be greater than 0.");

            RuleFor(x => x.VehicleStatusId)
                .GreaterThan(0)
                .WithMessage("Vehicle status is required.");

            // Optional date sanity checks (recommended)
            RuleFor(x => x.RegistrationExpiration)
                .Must(d => d == null || d.Value.Date > DateTime.UtcNow.Date)
                .WithMessage("Registration expiration must be a future date.")
                .When(x => x.RegistrationExpiration.HasValue);

            RuleFor(x => x.InsuranceExpiration)
                .Must(d => d == null || d.Value.Date > DateTime.UtcNow.Date)
                .WithMessage("Insurance expiration must be a future date.")
                .When(x => x.InsuranceExpiration.HasValue);
        }
    }
}
