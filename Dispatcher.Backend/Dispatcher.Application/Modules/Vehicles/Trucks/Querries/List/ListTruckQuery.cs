using MediatR;

namespace Dispatcher.Application.Modules.Vehicles.Trucks.Queries.List
{

    public sealed class ListTruckQuery : IRequest<List<ListTruckQueryDto>>
    {
        public string? Search { get; set; }
        public int? Status { get; set; }
    }
}