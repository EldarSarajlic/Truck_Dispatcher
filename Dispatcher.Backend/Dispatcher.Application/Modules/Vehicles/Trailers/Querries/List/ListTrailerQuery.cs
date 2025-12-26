using MediatR;

namespace Dispatcher.Application.Modules.Vehicles.Trailers.Queries.List
{
    public sealed class ListTrailerQuery : IRequest<List<ListTrailerQueryDto>>
    {
        public string? Search { get; set; }
        public int? Status { get; set; }
    }
}
