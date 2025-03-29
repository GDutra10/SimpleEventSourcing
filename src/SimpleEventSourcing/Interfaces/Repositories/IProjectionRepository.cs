namespace SimpleEventSourcing.Interfaces.Repositories;
public interface IProjectionRepository<TProjection>
    where TProjection : IProjection
{
    Task<TProjection?> GetAsync(Guid id, CancellationToken cancellationToken);
    Task SaveAsync(TProjection projection, CancellationToken cancellationToken);
}
