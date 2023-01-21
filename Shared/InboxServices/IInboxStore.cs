namespace Shared.InboxServices;

public interface IInboxStore<TMessage>
{
    Task Store(TMessage message, CancellationToken cancellationToken = default);
    Task<IEnumerable<TMessage>> GetUnsentMessages(CancellationToken cancellationToken = default);
    Task<TMessage?> GetNext(CancellationToken cancellationToken = default);
    Task MarkAsRecived(TMessage message, CancellationToken cancellationToken = default);
}