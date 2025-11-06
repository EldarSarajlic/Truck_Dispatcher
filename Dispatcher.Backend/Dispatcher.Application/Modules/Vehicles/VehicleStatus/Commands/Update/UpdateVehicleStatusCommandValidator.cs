namespace Dispatcher.Application.Modules.Vehicles.VehicleStatus.Commands.Update;

public sealed class UpdateVehicleStatusCommandValidator : AbstractValidator<UpdateVehicleStatusCommand>
{
    public UpdateVehicleStatusCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Id must be greater than zero.");

        RuleFor(x => x.StatusName)
            .NotEmpty().WithMessage("Status name is required.")
            .MaximumLength(100).WithMessage("Status name must not exceed 100 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");
    }
}