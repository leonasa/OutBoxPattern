using OutBoxPattern.Services;
using Shared.Contracts;

public interface IOrderConsumer
{
    Task<OrderMessage> Consume(CancellationToken cancellationToken = default);
}