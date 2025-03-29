using Example.Domain.Models.Requests;
using Example.InMemory.DifferentDictionary.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace Example.InMemory.DifferentDictionary.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
    private readonly OrderService _orderService;
    public OrderController(OrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrderAsync(OrderCreateRQ orderCreateRQ, CancellationToken cancellationToken)
        => Ok(await _orderService.CreateOrderAsync(orderCreateRQ, cancellationToken));

    [HttpGet("Events")]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
        => Ok(await _orderService.GetAllAsync(cancellationToken));
}
