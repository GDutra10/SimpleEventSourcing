namespace Example.Domain.Events;

public class UserUpdateEvent : UserEvent
{
    public override Guid StreamId => Id;
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string Email { get; init; }
}
