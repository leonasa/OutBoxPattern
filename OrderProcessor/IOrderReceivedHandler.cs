public interface IOrderReceivedHandler
{
    Task Handle(Order messagePlacedOrder, CancellationToken stoppingToken);
}