using System.Text.Json;
using FASTER.core;
using Shared.Contracts;
using Shared.InboxServices;
using Shared.OutboxServices;

namespace OrderProcessor;

public class OrderReceivedHandler : IOrderReceivedHandler
{
    
    private readonly FasterLog _fasterLog;

    public OrderReceivedHandler(FasterLog fasterLog)
    {
        _fasterLog= fasterLog;
    }

    public async Task Handle(Order messagePlacedOrder, CancellationToken cancellationToken)
    {
        var serializedOrderCreated = JsonSerializer.SerializeToUtf8Bytes(new OrderFulfilledMessage(messagePlacedOrder, DateTime.Now));
        await _fasterLog.EnqueueAsync(serializedOrderCreated, cancellationToken);
        await _fasterLog.CommitAsync(token: cancellationToken);
    }
}