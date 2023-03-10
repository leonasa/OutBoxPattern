using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shared.Contracts;

namespace Shared.InboxServices;

public class InboxProcessor : BackgroundService
{
    private readonly ILogger<InboxProcessor> _logger;
    private readonly IOrderReceivedHandler _orderReceivedHandler;
    private readonly IInboxStore<OrderMessage> _store;
    private readonly PeriodicTimer _timer;

    public InboxProcessor(IInboxStore<OrderMessage> store, ILogger<InboxProcessor> logger,
        IOrderReceivedHandler orderReceivedHandler, IOptions<InboxOptions> options)

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
            var message = await _store.GetNext(stoppingToken);
            if (message == null) continue;
            await _orderReceivedHandler.Handle(message.PlacedOrder, stoppingToken);
            await _store.MarkAsRecived(message, stoppingToken);
        }
    }
}