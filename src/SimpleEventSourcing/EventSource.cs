using System.Reflection;
using SimpleEventSourcing.Exceptions;
using SimpleEventSourcing.Interfaces;
using SimpleEventSourcing.Interfaces.Repositories;

namespace SimpleEventSourcing;
public class EventSource<TProjection, TEvent>
    where TProjection : IProjection, new()
    where TEvent : Event
{
    private static readonly Dictionary<Type, MethodInfo> MethodCache = new();

    private readonly IProjectionRepository<TProjection> _projectionRepository;
    private readonly IEventRepository<TEvent> _eventRepository;

    public EventSource(
        IProjectionRepository<TProjection> projectionRepository,
        IEventRepository<TEvent> eventRepository)
    {
        _projectionRepository = projectionRepository;
        _eventRepository = eventRepository;
    }

    public async Task AppendAsync(TEvent e, CancellationToken cancellationToken)
    {
        var projectionMethodInfo = GetMethodByReflection(e);

        e.CreatedAt = DateTime.UtcNow;
        var projection = await _projectionRepository.GetAsync(e.StreamId, cancellationToken) ?? new TProjection();

        projectionMethodInfo.Invoke(projection, [e]);

        await _eventRepository.AppendAsync(e, cancellationToken);
        await _projectionRepository.SaveAsync(projection, cancellationToken);
    }

    public async Task<TProjection?> GetAsync(Guid id, CancellationToken cancellationToken)
        => await _projectionRepository.GetAsync(id, cancellationToken);

    public async Task<List<TEvent>?> GetEventsByStreamIdAsync(Guid id, CancellationToken cancellationToken)
        => await _eventRepository.GetByStreamIdAsync(id, cancellationToken);

    public async Task<List<TEvent>> GetAllAsync(CancellationToken cancellationToken)
        => await _eventRepository.GetAllAsync(cancellationToken);

    public async Task<bool> HasProjectionAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = await _projectionRepository.GetAsync(id, cancellationToken);

        return result is not null;
    }

    private static MethodInfo GetMethodByReflection(Event e)
    {
        var eventType = e.GetType();

        if (MethodCache.TryGetValue(eventType, out var methodFromCache))
            return methodFromCache;

        var projectionType = typeof(TProjection);
        var projectionMethodInfo = projectionType
            .GetMethods()
            .FirstOrDefault(m =>
                m.ReturnType == typeof(void) &&
                m.GetParameters().Length == 1 &&
                m.GetParameters().FirstOrDefault(p => p.ParameterType == eventType) is not null);

        if (projectionMethodInfo is null)
            throw new ProjectionNotImplementedEventException(projectionType, e);

        MethodCache[eventType] = projectionMethodInfo;

        return projectionMethodInfo;
    }
}
