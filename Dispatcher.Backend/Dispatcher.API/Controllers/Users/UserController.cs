using Dispatcher.Application.Modules.Users.Queries.GetById;
using Dispatcher.Application.Modules.Users.Queries.List;

namespace Dispatcher.API.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController(ISender sender) : ControllerBase
{
    /// <summary>
    /// Get user by ID.
    /// </summary>
    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin,Dispatcher")]
    public async Task<GetUserByIdQueryDto> GetById(int id, CancellationToken ct)
    {
        var user = await sender.Send(new GetUserByIdQuery { Id = id }, ct);
        return user;
    }

    /// <summary>
    /// List users with optional filtering and pagination.
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Admin,Dispatcher")]
    public async Task<PageResult<ListUserQueryDto>> List([FromQuery] ListUserQuery query, CancellationToken ct)
    {
        var result = await sender.Send(query, ct);
        return result;
    }
}