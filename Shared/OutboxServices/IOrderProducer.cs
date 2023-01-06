using Confluent.Kafka;
using Shared.Contracts;

namespace Shared.OutboxServices;

public interface IProducer<TMessage>
{
    Task Produce(TMessage order, CancellationToken cancellationToken = default);
}

public interface IOrderProducer
{
    Task<DeliveryResult<string?, OrderMessage>> Produce(OrderMessage order,
        CancellationToken cancellationToken = default);
}
public interface IOrderOFulfilledProducer
{
    Task<DeliveryResult<string?, OrderFulfilledMessage>> Produce(OrderFulfilledMessage order,
        CancellationToken cancellationToken = default);
}