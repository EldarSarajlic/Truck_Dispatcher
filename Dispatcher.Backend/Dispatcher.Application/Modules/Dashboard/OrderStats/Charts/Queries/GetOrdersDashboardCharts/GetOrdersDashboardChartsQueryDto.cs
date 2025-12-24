namespace Dispatcher.Application.Modules.Dashboard.OrderStats.Charts.Queries.GetOrdersDashboardCharts
{
    /// <summary>
    /// Orders dashboard charts data grouped by month for a selected year.
    /// </summary>
    public class GetOrdersDashboardChartsQueryDto
    {
        /// <summary>
        /// Number of orders per month (index 0 = January).
        /// </summary>
        public int[] OrdersByMonth { get; set; } = new int[12];

        /// <summary>
        /// Revenue per month (index 0 = January).
        /// </summary>
        public decimal[] RevenueByMonth { get; set; } = new decimal[12];
    }
}
