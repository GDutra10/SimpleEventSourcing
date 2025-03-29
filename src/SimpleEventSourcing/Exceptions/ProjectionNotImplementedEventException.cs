namespace SimpleEventSourcing.Exceptions;
internal class ProjectionNotImplementedEventException(Type projectionType, Event e) : Exception(
    $"Projection '{projectionType.Name}' doest not have implementation for Event '{e.GetType().Name}'");
