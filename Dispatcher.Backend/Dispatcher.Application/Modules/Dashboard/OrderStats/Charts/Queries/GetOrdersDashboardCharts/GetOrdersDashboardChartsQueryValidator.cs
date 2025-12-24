using FluentValidation;

namespace Dispatcher.Application.Modules.Dashboard.OrderStats.Charts.Queries.GetOrdersDashboardCharts
{
    public sealed class GetOrdersDashboardChartsQueryValidator
        : AbstractValidator<GetOrdersDashboardChartsQuery>
    {
        public GetOrdersDashboardChartsQueryValidator()
        {
            RuleFor(x => x.Year)
                .GreaterThan(2000)
                .WithMessage("Year must be greater than 2000.")
                .LessThanOrEqualTo(DateTime.UtcNow.Year)
                .WithMessage("Year cannot be in the future.");
        }
    }
}
