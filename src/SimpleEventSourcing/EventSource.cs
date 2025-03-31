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
    private readonly EventSourcingConfiguration _eventSourcingConfiguration;

    public EventSource(
        IEnumerable<IEventHandler<TEvent, TProjection>> eventHandlers,
        IProjectionRepository<TProjection> projectionRepository,
        IEventRepository<TEvent> eventRepository,
        EventSourcingConfiguration eventSourcingConfiguration)
    {
        _projectionRepository = projectionRepository;
        _eventRepository = eventRepository;
        _eventSourcingConfiguration = eventSourcingConfiguration;
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
        var runCount = 0;

        do
        {
            runCount++;
            
            if (await TrySaveProjectionAsync(e, cancellationToken))
                break;

            if (runCount < _eventSourcingConfiguration.RetryTimes)
                await Task.Delay(_eventSourcingConfiguration.RetryDelayMs, cancellationToken);
            else
                throw new ConcurrencyException(e);
        } 
        while (true);

        await _eventRepository.AppendAsync(e, cancellationToken);
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

    private async Task<bool> TrySaveProjectionAsync(TEvent e, CancellationToken cancellationToken)
    {
        var projection = await _projectionRepository.GetAsync(e.StreamId, cancellationToken) ?? new TProjection();
        var originalVersion = projection.Version;
        var handler = GetHandler(e.GetType());

        if (handler is not null)
            handler.Handle(projection, e);
        else
            throw new EventHandlerNotFoundException(projection, e);

        e.CreatedAt = DateTime.UtcNow;

        var success = await _projectionRepository.TrySaveAsync(originalVersion, projection, cancellationToken);

        if (success)
            projection.Version++;

        return success;
    }

    private IEventHandler<TEvent, TProjection>? GetHandler(Type eventType)
    {
        return HandlerCache.GetOrAdd(eventType, type =>
        {
            var handler = _eventHandlers.TryGetValue(type, out var handlerObject)
                ? handlerObject as IEventHandler<TEvent, TProjection>
                : null;

            if (handler != null)
                return handler;

            return type.BaseType is not null ? GetHandler(type.BaseType) : null;
        });
    }
}
