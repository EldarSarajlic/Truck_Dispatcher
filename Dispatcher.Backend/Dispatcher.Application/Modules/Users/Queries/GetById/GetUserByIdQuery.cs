namespace Dispatcher.Application.Modules.Users.Queries.GetById;

public class GetUserByIdQuery : IRequest<GetUserByIdQueryDto>
{
    public int Id { get; set; }
}