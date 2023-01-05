using Microsoft.AspNetCore.Mvc;
using Shared.Contracts;
using Shared.OutboxServices;

namespace OutBoxPattern.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
    private readonly ILogger<OrderController> _logger;
    private readonly IOutboxStore<OrderMessage> _store;

    public OrderController(ILogger<OrderController> logger, IOutboxStore<OrderMessage> store)
    {
        _logger = logger;
        _store = store;
    }

    [HttpPost(Name = "CreateOrder")]
    public async Task<IActionResult> Post([FromBody] Order order)
    {
        if (ModelState.IsValid)
        {
            await _store.Store(new OrderMessage(order));
            return Ok();
        }
        return BadRequest();
    }
}