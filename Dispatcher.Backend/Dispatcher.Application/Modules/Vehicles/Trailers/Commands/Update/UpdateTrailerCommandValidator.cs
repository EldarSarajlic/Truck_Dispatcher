using FluentValidation;
using Dispatcher.Domain.Entities.Vehicles;

namespace Dispatcher.Application.Modules.Vehicles.Trailers.Commands.Update
{
    public class UpdateTrailerCommandValidator
        : AbstractValidator<UpdateTrailerCommand>
    {
        public UpdateTrailerCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0);

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
                .NotEmpty().WithMessage("Type is required.")
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
                .WithMessage("VehicleStatusId is required.");
        }
    }
}
