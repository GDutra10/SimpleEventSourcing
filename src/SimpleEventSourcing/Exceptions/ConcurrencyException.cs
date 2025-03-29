namespace SimpleEventSourcing.Exceptions;
public class ConcurrencyException(Event e) : Exception($"Projection {e.StreamId} was modified by another process.")
{
}
