namespace Shared.OutboxServices;

public interface IOutboxProcessor<TMessage>
{
    Task Process(CancellationToken stoppingToken);
}