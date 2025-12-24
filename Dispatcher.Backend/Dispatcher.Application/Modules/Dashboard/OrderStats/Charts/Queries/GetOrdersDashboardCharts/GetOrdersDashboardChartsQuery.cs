using MediatR;

namespace Dispatcher.Application.Modules.Dashboard.OrderStats.Charts.Queries.GetOrdersDashboardCharts
{
    /// <summary>
    /// Get yearly orders dashboard charts.
    /// </summary>
    public class GetOrdersDashboardChartsQuery
        : IRequest<GetOrdersDashboardChartsQueryDto>
    {
        public int Year { get; init; }
    }
}
