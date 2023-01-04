using Microsoft.Extensions.Options;
using Shared.Contracts;

public class InboxProcessor : BackgroundService
{
    private IInboxStore<OrderMessage> _store;
    private readonly ILogger<InboxProcessor> _logger;
    private IOrderReceivedHandler _orderReceivedHandler;
    private readonly PeriodicTimer _timer;
    
    public InboxProcessor(IInboxStore<OrderMessage> store, ILogger<InboxProcessor> logger, IOrderReceivedHandler orderReceivedHandler, IOptions<InboxOptions> options)
    
    {
        _store = store;
        _logger = logger;
        _orderReceivedHandler = orderReceivedHandler;
        _timer = new PeriodicTimer(options.Value.Interval);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (await _timer.WaitForNextTickAsync(stoppingToken))
        {
            var messages = await _store.GetUnsentMessages(stoppingToken);
            foreach (var message in messages)
            {
                await _orderReceivedHandler.Handle(message.PlacedOrder, stoppingToken);
                await _store.MarkAsSent(message, stoppingToken);
            }
        }
    }
}