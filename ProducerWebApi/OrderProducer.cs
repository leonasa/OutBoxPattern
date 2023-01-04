using Confluent.Kafka;
using Shared.Contracts;
using Shared.OutboxServices;

namespace OutBoxPattern;

public class OrderProducer : IOrderProducer
{
    private readonly IProducer<string?, OrderMessage> _producer;
    private readonly ILogger<OrderProducer> _logger;

    public OrderProducer(IProducer<string?, OrderMessage> producer, ILogger<OrderProducer> logger)
    {
        _producer = producer;
        _logger = logger;
    }

    public async Task Produce(OrderMessage order, CancellationToken cancellationToken = default)
    {
        var kafkaMessage = new Message<string?, OrderMessage>
        {
            Key = order.PlacedOrder.CustomerId,
            Value = order
        };

        var deliveryResult = await _producer.ProduceAsync(OrderMessage.Topic, kafkaMessage, cancellationToken);
        _logger.LogInformation($"Produced message with id {deliveryResult.Message.Key} to {deliveryResult.TopicPartitionOffset}");
    }
}