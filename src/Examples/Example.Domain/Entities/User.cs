using Example.Domain.Events;
using SimpleEventSourcing.Interfaces;

namespace Example.Domain.Entities;

public class User : IProjection
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public void Apply(UserCreateEvent createEvent)
    {
        Id = createEvent.UserId;
        Name = createEvent.Name;
        Email = createEvent.Email;
        Password = createEvent.Password;
    }

    public void Apply(UserUpdateEvent updateEvent)
    {
        Id = updateEvent.Id;
        Name = updateEvent.Name;
        Email = updateEvent.Email;
    }
}
