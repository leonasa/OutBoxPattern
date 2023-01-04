using Confluent.Kafka;

public class KafkaOrderConsumer : IOrderConsumer
{
    private readonly ILogger<KafkaOrderConsumer> _logger;
    private readonly IConsumer<string?, OrderMessage> _consumer;

    public KafkaOrderConsumer(ILogger<KafkaOrderConsumer> logger, IConsumer<string?, OrderMessage> consumer)
    {
        _consumer = consumer;
        _logger = logger;
        _consumer.Subscribe(OrderMessage.Topic);
    }
    
    public Task<OrderMessage> Consume(CancellationToken cancellationToken = default)
    {
        var result = _consumer.Consume(cancellationToken);
        _logger.LogInformation("Consumed message '{Message}' at: '{Timestamp}'", result.Message.Value, result.Message.Timestamp);
        return Task.FromResult(result.Message.Value);
    }
}