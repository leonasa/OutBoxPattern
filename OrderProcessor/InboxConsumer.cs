using Shared.Contracts;
using Shared.InboxServices;

namespace OrderProcessor;

public class InboxConsumer : BackgroundService
{
    private readonly IOrderConsumer _consumer;
    private readonly IInboxStore<OrderMessage> _inboxStore;

    public InboxConsumer(IInboxStore<OrderMessage> inboxStore, IOrderConsumer consumer)
    {
        _inboxStore = inboxStore;
        _consumer = consumer;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Run(async () =>
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var message = await _consumer.Consume(stoppingToken);
                await _inboxStore.Store(message, stoppingToken);
            }
        }, stoppingToken);
    }
}