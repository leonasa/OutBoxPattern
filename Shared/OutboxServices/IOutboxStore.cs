using System.Text;
using System.Text.Json;
using Confluent.Kafka;

namespace Shared.OutboxServices;

public interface IOutboxStore<TMessage>
{
    Task Store(TMessage message, CancellationToken cancellationToken = default);
    Task<IEnumerable<TMessage>> GetUnsentMessages( CancellationToken cancellationToken = default);
    Task<TMessage?> GetNext(CancellationToken cancellationToken = default);
    Task MarkAsSent(TMessage message, CancellationToken cancellationToken = default);
}


public class JsonSerializer<T> : IAsyncSerializer<T> where T : class
{
    public Task<byte[]> SerializeAsync(T data, SerializationContext context)
    {
        return Task.FromResult(JsonSerializer.SerializeToUtf8Bytes(data));
    }
}