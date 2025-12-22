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
