using SimpleEventSourcing.Interfaces.Repositories;
using SimpleEventSourcing.Interfaces;

namespace SimpleEventSourcing;
public abstract class ProjectionRepository<TProjection> : IProjectionRepository<TProjection>
    where TProjection : IProjection
{
    public async Task<TProjection?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        return await GetByIdAsync(id, cancellationToken);
    }

    public async Task<bool> TrySaveAsync(int originalVersion, TProjection projection, CancellationToken cancellationToken)
    {
        var projectionFromDb = await GetByIdAsync(projection.Id, cancellationToken);

        if (projectionFromDb is not null && projection.Version != originalVersion)
            return false;

        await SaveAsync(projection, cancellationToken);

        return true;
    }

    protected abstract Task<TProjection?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    protected abstract Task SaveAsync(TProjection projection, CancellationToken cancellationToken);
}
