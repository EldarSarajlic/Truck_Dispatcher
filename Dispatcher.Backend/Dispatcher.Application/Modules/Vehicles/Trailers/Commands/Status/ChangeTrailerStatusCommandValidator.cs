using FluentValidation;

namespace Dispatcher.Application.Modules.Vehicles.Trailers.Commands.Status.Change
{
    public class ChangeTrailerStatusCommandValidator
        : AbstractValidator<ChangeTrailerStatusCommand>
    {
        public ChangeTrailerStatusCommandValidator()
        {
            RuleFor(x => x.TrailerId)
                .GreaterThan(0);

            RuleFor(x => x.VehicleStatusId)
                .GreaterThan(0);
        }
    }
}
