using Shared.Contracts;
using Shared.InboxServices;

namespace OrderProcessor;

//NEED PRODUCE TO A TOPIC
public class OrderReceivedHandler : IOrderReceivedHandler
{
    public async Task Handle(Order messagePlacedOrder, CancellationToken stoppingToken)
    {
        throw new NotImplementedException();
    }
}