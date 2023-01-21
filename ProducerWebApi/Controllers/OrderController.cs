using System.Text;
using System.Text.Json;
using FASTER.core;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts;
using Shared.OutboxServices;

namespace OutBoxPattern.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
    private readonly FasterLog _fasterLog;
    private readonly ILogger<OrderController> _logger;

    public OrderController(ILogger<OrderController> logger, FasterLog fasterLog)
    {
        _fasterLog= fasterLog;
        _logger = logger;
    }

    [HttpPost(Name = "CreateOrder")]
    public async Task<IActionResult> Post([FromBody] Order order, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return BadRequest();
        
        var serializedOrderCreated = JsonSerializer.SerializeToUtf8Bytes(new OrderMessage(order));
        await _fasterLog.EnqueueAsync(serializedOrderCreated, cancellationToken);
        await _fasterLog.CommitAsync(token: cancellationToken);
        
        return Ok();
    }
}