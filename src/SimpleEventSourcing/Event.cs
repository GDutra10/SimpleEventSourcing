namespace SimpleEventSourcing;
public abstract class Event
{
    public abstract Guid StreamId { get; }
    public DateTime CreatedAt { get; set; }
}
