using Shared.Contracts;

namespace Shared.InboxServices;

public interface IOrderConsumer
{
    Task<OrderMessage> Consume(CancellationToken cancellationToken = default);
}