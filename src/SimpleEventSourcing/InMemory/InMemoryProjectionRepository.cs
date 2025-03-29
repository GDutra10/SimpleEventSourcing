using SimpleEventSourcing.Interfaces;

namespace SimpleEventSourcing.InMemory;

public class InMemoryProjectionRepository<TProjection> : ProjectionRepository<TProjection>
    where TProjection : IProjection
{
    private static readonly Dictionary<Guid, TProjection> Entities = [];

    protected override async Task<TProjection?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => await Task.FromResult(Entities.GetValueOrDefault(id));

    protected override async Task SaveAsync(TProjection projection, CancellationToken cancellationToken)
    {
        Entities[projection.Id] = projection;

        await Task.CompletedTask;
    }
}
