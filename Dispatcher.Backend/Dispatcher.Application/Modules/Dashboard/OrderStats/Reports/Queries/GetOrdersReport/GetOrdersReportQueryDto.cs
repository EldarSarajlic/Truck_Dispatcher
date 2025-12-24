namespace Dispatcher.Application.Modules.Dashboard.OrderStats.Reports.Queries.GetOrdersReport
{
    public class GetOrdersReportQueryDto
    {
        public string PeriodLabel { get; set; }

        // ===== FINANCE =====
        public int TotalOrders { get; set; }

        public decimal TotalRevenue { get; set; }
        public decimal AvgOrderValue { get; set; }
        public decimal MaxOrderValue { get; set; }
        public decimal MinOrderValue { get; set; }
        public Dictionary<string, decimal> RevenueByCurrency { get; set; } = new();

        // ===== ORDER FLOW =====
        public decimal ApprovalRate { get; set; }
        public decimal CancelRate { get; set; }
        public double? AvgDeliveryTimeDays { get; set; }
        public Dictionary<string, int> PriorityStats { get; set; } = new();

        // ===== ITEMS =====
        public decimal AvgItemsPerOrder { get; set; }
        public List<ReportItemDto> TopSellingItems { get; set; } = new();
        public List<ReportItemDto> TopProfitableItems { get; set; } = new();
        public List<ReportItemDto> MostCancelledItems { get; set; } = new();
    }

    public class ReportItemDto
    {
        public int InventoryId { get; set; }
        public string Name { get; set; }
        public decimal Quantity { get; set; }
        public decimal Revenue { get; set; }
    }
}
