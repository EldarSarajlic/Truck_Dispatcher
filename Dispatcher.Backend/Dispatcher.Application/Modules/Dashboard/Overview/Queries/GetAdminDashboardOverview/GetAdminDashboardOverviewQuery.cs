using Dispatcher.Application.Modules.Dashboard.Overview.Queries.GetAdminDashboardOverview;
using MediatR;

namespace Dispatcher.Application.Modules.Dashboard.Overview.Queries.GetAdminDashboardOverview
{
    public class GetAdminDashboardOverviewQuery 
        : IRequest<GetAdminDashboardOverviewQueryDto>
    {
        // Nema parametara
        // Ovo je snapshot admin dashboarda
    }
}
