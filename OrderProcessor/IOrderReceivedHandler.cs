using Shared.Contracts;

namespace OrderProcessor;

public interface IOrderReceivedHandler
{
    Task Handle(Order messagePlacedOrder, CancellationToken stoppingToken);
}