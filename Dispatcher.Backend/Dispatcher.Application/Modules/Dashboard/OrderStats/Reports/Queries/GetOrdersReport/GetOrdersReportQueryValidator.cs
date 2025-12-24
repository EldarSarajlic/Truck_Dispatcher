using FluentValidation;

namespace Dispatcher.Application.Modules.Dashboard.OrderStats.Reports.Queries.GetOrdersReport
{
    public sealed class GetOrdersReportQueryValidator
        : AbstractValidator<GetOrdersReportQuery>
    {
        public GetOrdersReportQueryValidator()
        {
            RuleFor(x => x.Year)
                .GreaterThan(2000)
                .LessThanOrEqualTo(DateTime.UtcNow.Year);

            When(x => x.Month.HasValue, () =>
            {
                RuleFor(x => x.Month!.Value)
                    .InclusiveBetween(1, 12);
            });
        }
    }
}
