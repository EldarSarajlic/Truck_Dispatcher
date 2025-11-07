using Dispatcher.Domain.Entities.Shipments;
using FluentValidation;

public class CreateShipmentCommandValidator : AbstractValidator<CreateShipmentCommand>
{
    public CreateShipmentCommandValidator()
    {
        RuleFor(x => x.Weight)
            .GreaterThan(0).WithMessage("Težina mora biti veća od 0.");

        RuleFor(x => x.Volume)
            .GreaterThan(0).WithMessage("Volumen mora biti veći od 0.");

        RuleFor(x => x.PickupLocation)
            .NotEmpty().WithMessage("PickupLocation je obavezno polje.")
            .MaximumLength(ShipmentEntity.Constraints.PickupLocationMaxLength);

        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Status je obavezan.")
            .MaximumLength(ShipmentEntity.Constraints.StatusMaxLength);

        RuleFor(x => x.Description)
            .MaximumLength(ShipmentEntity.Constraints.DescriptionMaxLength);
    }
}