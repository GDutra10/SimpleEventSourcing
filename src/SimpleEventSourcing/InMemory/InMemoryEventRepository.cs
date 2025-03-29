using SimpleEventSourcing.Interfaces.Repositories;

namespace SimpleEventSourcing.InMemory;

public class InMemoryEventRepository<TEvent> : IEventRepository<TEvent> where TEvent : Event
{
    private static readonly SortedDictionary<Guid, List<TEvent>> EventDb = new ();

    public async Task AppendAsync(TEvent e, CancellationToken cancellationToken)
    {
        if (!EventDb.ContainsKey(e.StreamId))
            EventDb[e.StreamId] = [];

        EventDb[e.StreamId].Add(e);

        await Task.CompletedTask;
    }

    public async Task<List<TEvent>> GetAllAsync(CancellationToken cancellationToken)
        => await Task.FromResult(EventDb.Values.SelectMany(list => list).ToList());

    public async Task<List<TEvent>?> GetByStreamIdAsync(Guid streamId, CancellationToken cancellationToken)
        => await Task.FromResult(EventDb.GetValueOrDefault(streamId));
}
