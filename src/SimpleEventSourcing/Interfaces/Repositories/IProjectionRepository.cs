namespace SimpleEventSourcing.Interfaces.Repositories;
public interface IProjectionRepository<TProjection>
    where TProjection : IProjection
{
    Task<TProjection?> GetAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> TrySaveAsync(int originalVersion, TProjection projection, CancellationToken cancellationToken);
}
