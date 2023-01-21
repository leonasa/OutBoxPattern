using System.Text;
using Confluent.Kafka;
using FASTER.core;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shared.Contracts;

namespace Shared.OutboxServices;

public sealed class OutboxFulfilledProcessor : BackgroundService
{
    private readonly ILogger<OutboxProcessor> _logger;
    private readonly OutboxOptions _options;
    private readonly IOrderOFulfilledProducer _orderProducer;
    private readonly FasterLog _fasterLog;
    private readonly PeriodicTimer _timer;

    public OutboxFulfilledProcessor(IOrderOFulfilledProducer orderProducer,
        FasterLog fasterLog, ILogger<OutboxProcessor> logger,
        IOptions<OutboxOptions> options)
    {
        _orderProducer = orderProducer;
        _fasterLog = fasterLog;
        _logger = logger;
        _options = options.Value;
        _timer = new PeriodicTimer(_options.Interval);
    }

    async Task ConsumerAsync(CancellationToken cancellationToken)
    {
        using var iter = _fasterLog.Scan(_fasterLog.BeginAddress, long.MaxValue, "consumerLogIter", true, ScanBufferingMode.DoublePageBuffering, true);

        try
        {
            int count = 0;
            await foreach (var (result, length, currentAddress, nextAddress) in iter.GetAsyncEnumerable(cancellationToken))
            {
                Console.WriteLine($"Same Log Consuming {Encoding.UTF8.GetString(result)}");
                var order = System.Text.Json.JsonSerializer.Deserialize<OrderFulfilledMessage>(Encoding.UTF8.GetString(result));
                try
                {
                    _logger.LogInformation("Producing order from store");
                    var deliveryResult = await _orderProducer.Produce(order, cancellationToken);
                    if (deliveryResult.Status == PersistenceStatus.Persisted)
                    {
                        _logger.LogInformation("Order produced successfully");
                        iter.CompleteUntil(nextAddress);
                        _fasterLog.TruncateUntil(nextAddress);
                    }
                    else
                    {
                        _logger.LogWarning("Order produced with status {Status}", deliveryResult.Status);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }
        catch (OperationCanceledException) { }
        Console.WriteLine("Consumer complete");
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Run(async () =>
        {
            await ConsumerAsync(stoppingToken);
        }, stoppingToken);
    }
}