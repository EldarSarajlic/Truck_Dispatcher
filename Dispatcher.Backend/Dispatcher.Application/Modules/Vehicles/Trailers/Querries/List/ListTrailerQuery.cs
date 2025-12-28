using Dispatcher.Application.Common;

namespace Dispatcher.Application.Modules.Vehicles.Trailers.Queries.List
{
    public sealed class ListTrailerQuery : BasePagedQuery<ListTrailerQueryDto>
    {
        /// <summary>
        /// Free text search (license plate, make, model, type)
        /// </summary>
        public string? Search { get; init; }

        /// <summary>
        /// Filter by vehicle status
        /// </summary>
        public int? Status { get; init; }
    }
}
