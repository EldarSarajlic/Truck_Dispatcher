// === QUERIES (READ) ===

/**
 * Response for GET /Dashboard/overview
 * Corresponds to: GetAdminDashboardOverviewQueryDto.cs
 */
export interface GetAdminDashboardOverviewResponse {
  totalSales: number;
  totalOrders: number;
  totalUsers: number;
  pendingOrders: number;
  recentOrders: RecentOrderItem[];
}

/**
 * Item for Recent Orders list (part of dashboard overview)
 * Corresponds to: RecentOrderDto.cs
 */
export interface RecentOrderItem {
  reference: string;
  client: string;
  price: number;
  date: string; // ISO Date string
  status: string
}

// === ORDERS DASHBOARD ===

/**
 * Response for GET /dashboard/orders/summary
 * Corresponds to: GetOrdersDashboardSummaryQueryDto.cs
 */
export interface GetOrdersDashboardSummaryResponse {
  totalRevenue: number;
  monthlyRevenue: number;
  totalOrders: number;
  avgOrderValue: number;
}

/**
 * Response for GET /dashboard/orders/charts
 */
export interface GetOrdersDashboardChartsResponse {
  ordersByMonth: number[];   // length = 12
  revenueByMonth: number[];  // length = 12
}

export interface GetOrdersReportResponse {
  periodLabel: string;

  // ===== FINANCE =====
  totalOrders: number;
  totalRevenue: number;
  avgOrderValue: number;
  maxOrderValue: number;
  minOrderValue: number;
  revenueByCurrency: Record<string, number>;

  // ===== ORDER FLOW =====
  approvalRate: number;
  cancelRate: number;
  avgDeliveryTimeDays?: number;
  priorityStats: Record<string, number>;

  // ===== ITEMS =====
  avgItemsPerOrder: number;
  topSellingItems: ReportItem[];
  topProfitableItems: ReportItem[];
  mostCancelledItems: ReportItem[];
}

export interface ReportItem {
  inventoryId: number;
  name: string;
  quantity: number;
  revenue: number;
}
