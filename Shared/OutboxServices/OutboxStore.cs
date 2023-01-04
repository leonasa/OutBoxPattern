using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Shared.OutboxServices;


public interface IOutboxProcessor<TMessage>
{
    Task Process(CancellationToken stoppingToken);
}

public sealed class OutboxProcessorGen<TMessage> : IOutboxProcessor<TMessage>
{
    private readonly IProducer<TMessage> _orderProducer;
    private readonly IOutboxStore<TMessage> _outboxStore;
    private readonly ILogger<OutboxProcessorGen<TMessage>> _logger;
    private readonly OutboxOptions _options;
    private readonly PeriodicTimer _timer;

    public OutboxProcessorGen(IProducer<TMessage> orderProducer, IOutboxStore<TMessage> outboxStore, ILogger<OutboxProcessorGen<TMessage>> logger, IOptions<OutboxOptions> options)
    {
        _orderProducer = orderProducer;
        _outboxStore = outboxStore;
        _logger = logger;
        _options = options.Value;
        _timer = new PeriodicTimer(_options.Interval);
    }
    
    public async Task Process(CancellationToken stoppingToken)
    {
        while (await _timer.WaitForNextTickAsync(stoppingToken))
        {
            TMessage? order = await _outboxStore.GetNext(stoppingToken);
            if (order is null)
            {
                continue;
            }

            try
            {
                _logger.LogInformation("Adding order to store");
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

public class OutboxStore<TMessage> : IOutboxStore<TMessage>
{
    private readonly List<TMessage> _messages = new List<TMessage>();
    public Task Store(TMessage message, CancellationToken cancellationToken = default)
    {
        _messages.Add(message);
        return Task.CompletedTask;
    }

    public Task<IEnumerable<TMessage>> GetUnsentMessages(CancellationToken cancellationToken = default)
    {
        return  Task.FromResult(_messages.AsEnumerable());
    }

    public Task<TMessage?> GetNext(CancellationToken cancellationToken = default)
    {
        return  Task.FromResult(_messages.FirstOrDefault());
    }

    public Task MarkAsSent(TMessage message, CancellationToken cancellationToken = default)
    {
        _messages.Remove(message);
        return Task.CompletedTask;
    }
}