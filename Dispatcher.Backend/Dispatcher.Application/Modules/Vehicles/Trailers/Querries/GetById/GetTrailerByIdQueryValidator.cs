

using Dispatcher.Application.Modules.Vehicles.Trailers.Queries.GetById;

public sealed class GetTrailerByIdQueryValidator : AbstractValidator<GetTrailerByIdQuery>
{
    public GetTrailerByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Id must be greater than zero.");
    }
}