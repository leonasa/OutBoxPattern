namespace OutBoxPattern.Services;

public interface IOrderProducer
{
    Task Produce(OrderMessage order, CancellationToken cancellationToken = default);
}