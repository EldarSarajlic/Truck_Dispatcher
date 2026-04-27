using Dispatcher.Application.Abstractions;
using Dispatcher.Application.Modules.Users.Commands.Update;
using Dispatcher.Application.Modules.Users.Commands.UploadPhoto;
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

    /// <summary>
    /// Update an existing user by ID.
    /// </summary>
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Update(int id, UpdateUserCommand command, CancellationToken ct)
    {
        if (id != command.Id) return BadRequest();
        await sender.Send(command, ct);
        return NoContent();
    }

    /// <summary>
    /// Upload or replace a user's profile photo.
    /// </summary>
    [HttpPost("{id:int}/photo")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> UploadPhoto(int id, IFormFile photo, CancellationToken ct)
    {
        if (photo is null || photo.Length == 0)
            return BadRequest("No photo provided.");

        await using var stream = photo.OpenReadStream();
        var photoFile = new PhotoFile(stream, photo.FileName, photo.ContentType, photo.Length);

        await sender.Send(new UploadUserPhotoCommand { UserId = id, Photo = photoFile }, ct);
        return NoContent();
    }
}