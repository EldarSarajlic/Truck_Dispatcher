using FluentValidation;

namespace Dispatcher.Application.Modules.Shipments.Shipment.Querries.GetById
{
    public class GetShipmentByIdQueryValidator : AbstractValidator<GetShipmentByIdQuery>
    {
        public GetShipmentByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("ID shipmenta mora biti veći od nule.");
        }
    }
}