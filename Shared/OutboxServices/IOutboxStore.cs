namespace OutBoxPattern.Services;

public interface IOutboxStore<TMessage>
{
    Task Store(TMessage message, CancellationToken cancellationToken = default);
    Task<IEnumerable<TMessage>> GetUnsentMessages( CancellationToken cancellationToken = default);
    Task<TMessage?> GetNext(CancellationToken cancellationToken = default);
    Task MarkAsSent(TMessage message, CancellationToken cancellationToken = default);
}
