using Shared.Contracts;

namespace Shared.InboxServices;

public interface IOrderReceivedHandler
{
    Task Handle(Order messagePlacedOrder, CancellationToken stoppingToken);
}