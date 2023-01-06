using Shared.Contracts;
using Shared.InboxServices;
using Shared.OutboxServices;

namespace OrderProcessor;
public class OrderReceivedHandler : IOrderReceivedHandler
{
    private readonly IOutboxStore<OrderFulfilledMessage> _store;

    public OrderReceivedHandler(IOutboxStore<OrderFulfilledMessage> store)
    {
        _store = store;
    }

    public async Task Handle(Order messagePlacedOrder, CancellationToken stoppingToken)
    {
        _store.Store(new OrderFulfilledMessage(messagePlacedOrder, DateTime.Now));
    }
}