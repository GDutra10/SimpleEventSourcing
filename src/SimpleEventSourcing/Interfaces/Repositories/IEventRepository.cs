namespace SimpleEventSourcing.Interfaces.Repositories;
public interface IEventRepository<TEvent> where TEvent : Event
{
    Task AppendAsync(TEvent e, CancellationToken cancellationToken);
    Task<List<TEvent>> GetAllAsync(CancellationToken cancellationToken);
    Task<List<TEvent>?> GetByStreamIdAsync(Guid streamId, CancellationToken cancellationToken);
}
