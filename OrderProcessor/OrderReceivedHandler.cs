using Shared.Contracts;

namespace OrderProcessor;

public class OrderReceivedHandler : IOrderReceivedHandler
{
    public async Task Handle(Order messagePlacedOrder, CancellationToken stoppingToken)
    {
        throw new NotImplementedException();
    }
}