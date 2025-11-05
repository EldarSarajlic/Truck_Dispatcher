using FluentValidation;
using Dispatcher.Domain.Entities.Vehicles;
using Dispatcher.Application.Modules.Vehicles.VehicleStatus.Commands.Create;

namespace Dispatcher.Application.Modules.Vehicles.TruckStatuses.Commands.Create
{
    /// <summary>
    /// Validator for CreateVehicleStatusCommand.
    /// </summary>
    public sealed class CreateVehicleStatusCommandValidator
        : AbstractValidator<CreateVehicleStatusCommand>
    {
        public CreateVehicleStatusCommandValidator()
        {
            RuleFor(x => x.StatusName)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(VehicleStatusEntity.Constraints.StatusNameMaxLength)
                .WithMessage($"Name can be at most {VehicleStatusEntity.Constraints.StatusNameMaxLength} characters long.");
        }
    }
}
