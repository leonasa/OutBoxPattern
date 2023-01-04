public interface IOrderConsumer
{
    Task<OrderMessage> Consume(CancellationToken cancellationToken = default);
}