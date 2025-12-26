using MediatR;

namespace Dispatcher.Application.Modules.Vehicles.Trailers.Queries.GetById
{
    public class GetTrailerByIdQuery : IRequest<GetTrailerByIdQueryDto?>
    {
        public int Id { get; set; }
    }
}
