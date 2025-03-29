using SimpleEventSourcing.Interfaces;

namespace SimpleEventSourcing.Exceptions;
internal class EventHandlerNotFoundException(IProjection projection, Event e) : Exception(
    $"EventHandler not found! Projection: '{projection.GetType().Name}', Event: '{e.GetType().Name}'");
