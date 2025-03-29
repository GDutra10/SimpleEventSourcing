namespace SimpleEventSourcing.Interfaces;
public interface IProjection
{
    Guid Id { get; set; }
    public int Version { get; set; }
}
