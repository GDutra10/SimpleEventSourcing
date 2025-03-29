namespace SimpleEventSourcing.Interfaces;
public interface IEventHandler<in TEvent, in TProjection>
    where TEvent : Event
    where TProjection : IProjection
{
    void Handle(TProjection projection, TEvent e);
}