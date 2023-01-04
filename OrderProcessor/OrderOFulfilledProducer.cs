using Confluent.Kafka;
using Shared.Contracts;
using Shared.OutboxServices;

namespace OrderProcessor;

public class OrderOFulfilledProducer : IOrderOFulfilledProducer
{
    private readonly IProducer<string?, OrderFulfilledMessage> _producer;
    private readonly ILogger<OrderOFulfilledProducer> _logger;

    public OrderOFulfilledProducer(IProducer<string?, OrderFulfilledMessage> producer, ILogger<OrderOFulfilledProducer> logger)
    {
        _producer = producer;
        _logger = logger;
    }

    public async Task Produce(OrderFulfilledMessage order, CancellationToken cancellationToken = default)
    {
        var kafkaMessage = new Message<string?, OrderFulfilledMessage>
        {
            Key = order.PlacedOrder.CustomerId,
            Value = order
        };

        var deliveryResult = await _producer.ProduceAsync(OrderFulfilledMessage.Topic, kafkaMessage, cancellationToken);
        _logger.LogInformation($"Produced message with id {deliveryResult.Message.Key} to {deliveryResult.TopicPartitionOffset}");
    }
}