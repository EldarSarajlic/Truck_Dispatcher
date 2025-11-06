using Dispatcher.Domain.Entities.Vehicles;

namespace Dispatcher.Application.Modules.Vehicles.Trucks.Commands.Update
{
    public class UpdateTruckCommandValidator : AbstractValidator<UpdateTruckCommand>
    {
        public UpdateTruckCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Truck ID is required.");

            RuleFor(x => x.LicensePlateNumber)
                .NotEmpty().WithMessage("License plate number is required.")
                .MaximumLength(TruckEntity.Constraints.LicensePlateMaxLength);

            RuleFor(x => x.VinNumber)
                .NotEmpty().WithMessage("VIN number is required.")
                .MaximumLength(TruckEntity.Constraints.VinNumberMaxLength);

            RuleFor(x => x.Make)
                .NotEmpty().WithMessage("Make is required.")
                .MaximumLength(TruckEntity.Constraints.MakeMaxLength);

            RuleFor(x => x.Model)
                .NotEmpty().WithMessage("Model is required.")
                .MaximumLength(TruckEntity.Constraints.ModelMaxLength);

            RuleFor(x => x.Year)
                .GreaterThan(1980).WithMessage("Year must be greater than 1980."); // prilagodi po potrebi

            RuleFor(x => x.Capacity)
                .GreaterThan(0).WithMessage("Capacity must be greater than 0.");

            RuleFor(x => x.EngineCapacity)
                .GreaterThan(0).WithMessage("Engine capacity must be greater than 0.");

            RuleFor(x => x.KW)
                .GreaterThan(0).WithMessage("KW must be greater than 0.");

            // Add more validation rules as needed!
        }
    }
}