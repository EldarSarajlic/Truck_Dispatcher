namespace Dispatcher.Application.Modules.Vehicles.Trailers.Commands.Delete;

public class DeleteTrailerCommand : IRequest<Unit>
{
    public required int Id { get; set; }
}