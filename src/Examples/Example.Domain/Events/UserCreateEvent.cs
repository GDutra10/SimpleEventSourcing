namespace Example.Domain.Events;

public class UserCreateEvent : UserEvent
{
    public override Guid StreamId => UserId;
    public required Guid UserId { get; init; }
    public required string Name { get; init; }
    public required string Email { get; init; } 
    public required string Password { get; init; }
}
