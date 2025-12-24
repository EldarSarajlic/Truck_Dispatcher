namespace Dispatcher.Application.Modules.Dashboard.OrderStats.Summary.Queries.GetOrdersDashboardSummary
{
    /// <summary>
    /// Orders dashboard KPI summary.
    /// - TotalRevenue: Year-to-date revenue (current year)
    /// - MonthlyRevenue: Current month revenue
    /// </summary>
    public class GetOrdersDashboardSummaryQueryDto
    {
        /// <summary>
        /// Total revenue for the current year (year-to-date).
        /// </summary>
        public decimal TotalRevenue { get; set; }

        /// <summary>
        /// Revenue for the current month.
        /// </summary>
        public decimal MonthlyRevenue { get; set; }

        /// <summary>
        /// Total number of orders in the current month.
        /// </summary>
        public int TotalOrders { get; set; }

        /// <summary>
        /// Average order value for the current month.
        /// </summary>
        public decimal AvgOrderValue { get; set; }
    }
}
