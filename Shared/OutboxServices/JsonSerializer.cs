using System.Text.Json;
using Confluent.Kafka;

namespace Shared.OutboxServices;

public class JsonSerializer<T> : IAsyncSerializer<T> where T : class
{
    public Task<byte[]> SerializeAsync(T data, SerializationContext context)
    {
        return Task.FromResult(JsonSerializer.SerializeToUtf8Bytes(data));
    }
}
public class JsonDeserializer<T> : IAsyncDeserializer<T> where T : class
{
    public Task<T> DeserializeAsync(ReadOnlyMemory<byte> data, bool isNull, SerializationContext context)
    {
        return Task.FromResult(JsonSerializer.Deserialize<T>(data.Span));
    }
}