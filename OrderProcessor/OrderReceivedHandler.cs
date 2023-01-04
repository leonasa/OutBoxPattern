using Shared.Contracts;

public class OrderReceivedHandler : IOrderReceivedHandler
{
    public async Task Handle(Order messagePlacedOrder, CancellationToken stoppingToken)
    {
        throw new NotImplementedException();
    }
}