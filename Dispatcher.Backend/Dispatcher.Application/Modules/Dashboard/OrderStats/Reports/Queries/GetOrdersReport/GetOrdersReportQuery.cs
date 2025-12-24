using MediatR;

namespace Dispatcher.Application.Modules.Dashboard.OrderStats.Reports.Queries.GetOrdersReport
{
    /// <summary>
    /// Get monthly or yearly orders report.
    /// </summary>
    public class GetOrdersReportQuery
        : IRequest<GetOrdersReportQueryDto>
    {
        public int Year { get; init; }
        public int? Month { get; init; }
    }
}
