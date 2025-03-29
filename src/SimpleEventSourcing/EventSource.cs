using SimpleEventSourcing.Exceptions;
using SimpleEventSourcing.Interfaces;
using SimpleEventSourcing.Interfaces.Repositories;
using System.Collections.Concurrent;

namespace SimpleEventSourcing;
public class EventSource<TProjection, TEvent>
    where TProjection : IProjection, new()
    where TEvent : Event
{
    private static readonly ConcurrentDictionary<Type, IEventHandler<TEvent, TProjection>?> HandlerCache = new();
    private readonly Dictionary<Type, object> _eventHandlers;
    private readonly IProjectionRepository<TProjection> _projectionRepository;
    private readonly IEventRepository<TEvent> _eventRepository;

    public EventSource(
        IEnumerable<IEventHandler<TEvent, TProjection>> eventHandlers,
        IProjectionRepository<TProjection> projectionRepository,
        IEventRepository<TEvent> eventRepository)
    {
        _projectionRepository = projectionRepository;
        _eventRepository = eventRepository;
        _eventHandlers = eventHandlers.ToDictionary(
            handler => handler.GetType()
                .GetInterfaces()
                .First(i => i.IsGenericType &&
                            i.GetGenericTypeDefinition() == typeof(IEventHandler<,>))
                .GetGenericArguments()[0],
            handler => (object)handler);
    }

    public async Task AppendAsync(TEvent e, CancellationToken cancellationToken)
    {
        var projection = await _projectionRepository.GetAsync(e.StreamId, cancellationToken) ?? new TProjection();
        var handler = GetHandler(e.GetType());

        if (handler is not null)
            handler.Handle(projection, e);
        else
            throw new EventHandlerNotFoundException(projection, e);

        e.CreatedAt = DateTime.UtcNow;

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

    private IEventHandler<TEvent, TProjection>? GetHandler(Type eventType)
    {
        return HandlerCache.GetOrAdd(eventType, type => _eventHandlers.TryGetValue(type, out var handlerObject)
            ? handlerObject as IEventHandler<TEvent, TProjection>
            : type.BaseType is not null
                ? GetHandler(type.BaseType)
                : null);
    }
}
