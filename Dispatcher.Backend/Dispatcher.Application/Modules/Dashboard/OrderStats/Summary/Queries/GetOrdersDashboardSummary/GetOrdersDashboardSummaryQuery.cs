using MediatR;

namespace Dispatcher.Application.Modules.Dashboard.OrderStats.Summary.Queries.GetOrdersDashboardSummary
{
    /// <summary>
    /// Get current month orders dashboard KPI summary.
    /// </summary>
    public class GetOrdersDashboardSummaryQuery 
        : IRequest<GetOrdersDashboardSummaryQueryDto>
    {
    }
}
