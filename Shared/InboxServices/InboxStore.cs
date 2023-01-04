namespace Shared.InboxServices;

public class InboxStore<TMessage> : IInboxStore<TMessage>
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