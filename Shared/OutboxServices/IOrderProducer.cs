using Shared.Contracts;

namespace OutBoxPattern.Services;

public interface IProducer<TMessage>
{
    Task Produce(TMessage order, CancellationToken cancellationToken = default);
}

public interface IOrderProducer
{
    Task Produce(OrderMessage order, CancellationToken cancellationToken = default);
}
public interface IOrderOFulfilledProducer
{
    Task Produce(OrderFulfilledMessage order, CancellationToken cancellationToken = default);
}