using Example.Domain.Events;
using SimpleEventSourcing.Interfaces;

namespace Example.Domain.Entities;

public class User : IProjection
{
    public Guid Id { get; set; }
    public int Version { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
