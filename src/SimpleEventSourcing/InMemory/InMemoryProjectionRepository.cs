using SimpleEventSourcing.Interfaces;
using SimpleEventSourcing.Interfaces.Repositories;

namespace SimpleEventSourcing.InMemory;

public class InMemoryProjectionRepository<TProjection> : IProjectionRepository<TProjection> 
    where TProjection : IProjection
{
    private static readonly Dictionary<Guid, TProjection> Entities = [];

    public async Task<TProjection?> GetAsync(Guid id, CancellationToken cancellationToken)
        => await Task.FromResult(Entities.GetValueOrDefault(id));

    public async Task SaveAsync(TProjection projection, CancellationToken cancellationToken)
    {
        Entities[projection.Id] = projection;
        
        await Task.CompletedTask;
    }
}
