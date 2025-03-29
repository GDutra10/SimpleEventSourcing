using Example.Domain.Models.Requests;
using Example.InMemory.DifferentDictionary.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace Example.InMemory.DifferentDictionary.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;
    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> SaveAsync(UserCreateRQ userCreateRQ, CancellationToken cancellationToken)
    {
        var result = await _userService.CreateUserAsync(userCreateRQ, cancellationToken);

        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromQuery]Guid id, UserUpdateRQ userUpdateRQ, CancellationToken cancellationToken)
    {
        var result = await _userService.UpdateUserAsync(id, userUpdateRQ, cancellationToken);

        return result is not null ? Ok(result) : NotFound("User not found!");
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync([FromQuery] Guid id, CancellationToken cancellationToken)
    {
        var result = await _userService.GetAsync(id, cancellationToken);

        return result is not null ? Ok(result) : NotFound("User not found!");
    }

    [HttpGet("{userId}/Events")]
    public async Task<IActionResult> GetEventsAsync([FromRoute] Guid userId, CancellationToken cancellationToken)
    {
        var result = await _userService.GetEventsAsync(userId, cancellationToken);

        return result is not null ? Ok(result) : NotFound("User not found!");
    }

    [HttpGet("Events")]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
        => Ok(await _userService.GetAllAsync(cancellationToken));
}
