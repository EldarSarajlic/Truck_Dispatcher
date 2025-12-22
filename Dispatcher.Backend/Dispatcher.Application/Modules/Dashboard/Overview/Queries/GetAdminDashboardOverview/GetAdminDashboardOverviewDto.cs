namespace Dispatcher.Application.Modules.Dashboard.Overview.Queries.GetAdminDashboardOverview
{
    public class GetAdminDashboardOverviewQueryDto
    {
        public decimal TotalSales { get; set; }
        public int TotalOrders { get; set; }
        public int TotalUsers { get; set; }
        public int PendingOrders { get; set; }

        public List<RecentOrderDto> RecentOrders { get; set; } = new();
    }

    public class RecentOrderDto
    {
        public string Reference { get; set; }
        public string Client { get; set; }
        public decimal Price { get; set; }
        public DateTime Date { get; set; }
    }
}