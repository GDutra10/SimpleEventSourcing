using Microsoft.AspNetCore.Mvc;

namespace Example.InMemory.SameDictionary.WebAPI.Controllers;
[ApiController]
[Route("[controller]")]
public class StorageController : ControllerBase
{
    private readonly StorageService _storageService;

    public StorageController(StorageService storageService)
    {
        _storageService = storageService;
    }

    [HttpPost]
    public async Task<IActionResult> SaveEventsAsync(CancellationToken cancellationToken)
    {
        await _storageService.SaveEventsAsync(cancellationToken);

        return Ok();
    }

    [HttpGet("Order/Events")]
    public async Task<IActionResult> GetOrderEventsAsync(CancellationToken cancellationToken)
        => Ok(await _storageService.GetAllOrderEventsAsync(cancellationToken));

    [HttpGet("User/Events")]
    public async Task<IActionResult> GetUserEventsAsync(CancellationToken cancellationToken)
        => Ok(await _storageService.GetAllUserEventsAsync(cancellationToken));
}
