using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shared.Contracts;

namespace Shared.OutboxServices;

public sealed class OutboxProcessor : BackgroundService
{
    private readonly IOrderProducer _orderProducer;
    private readonly IOutboxStore<OrderMessage> _outboxStore;
    private readonly ILogger<OutboxProcessor> _logger;
    private readonly OutboxOptions _options;
    private readonly PeriodicTimer _timer;

    public OutboxProcessor(IOrderProducer orderProducer, IOutboxStore<OrderMessage> outboxStore, ILogger<OutboxProcessor> logger, IOptions<OutboxOptions> options)
    {
        _orderProducer = orderProducer;
        _outboxStore = outboxStore;
        _logger = logger;
        _options = options.Value;
        _timer = new PeriodicTimer(_options.Interval);
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // return Task.Run(async () =>
        // {
        //     while (!stoppingToken.IsCancellationRequested)
        //     {
        //         await _timer.WaitForNextTick(stoppingToken);
        //         await Process(stoppingToken);
        //     }
        // }, stoppingToken);

        while (await _timer.WaitForNextTickAsync(stoppingToken))
        {
            OrderMessage? order = await _outboxStore.GetNext(stoppingToken);
            if (order is null)
            {
                continue;
            }

            try
            {
                _logger.LogInformation("Producing order from store");
                await _orderProducer.Produce(order, stoppingToken);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
public sealed class OutboxFulfilledProcessor : BackgroundService
{
    private readonly IOrderOFulfilledProducer _orderProducer;
    private readonly IOutboxStore<OrderFulfilledMessage> _outboxStore;
    private readonly ILogger<OutboxProcessor> _logger;
    private readonly OutboxOptions _options;
    private readonly PeriodicTimer _timer;

    public OutboxFulfilledProcessor(IOrderOFulfilledProducer orderProducer, IOutboxStore<OrderFulfilledMessage> outboxStore, ILogger<OutboxProcessor> logger, IOptions<OutboxOptions> options)
    {
        _orderProducer = orderProducer;
        _outboxStore = outboxStore;
        _logger = logger;
        _options = options.Value;
        _timer = new PeriodicTimer(_options.Interval);
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // return Task.Run(async () =>
        // {
        //     while (!stoppingToken.IsCancellationRequested)
        //     {
        //         await _timer.WaitForNextTick(stoppingToken);
        //         await Process(stoppingToken);
        //     }
        // }, stoppingToken);

        while (await _timer.WaitForNextTickAsync(stoppingToken))
        {
            OrderFulfilledMessage? order = await _outboxStore.GetNext(stoppingToken);
            if (order is null)
            {
                continue;
            }

            try
            {
                _logger.LogInformation("Producing order from store");
                await _orderProducer.Produce(order, stoppingToken);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}