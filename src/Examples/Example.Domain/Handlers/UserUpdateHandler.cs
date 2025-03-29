using Example.Domain.Events;
using SimpleEventSourcing.Interfaces;
using Example.Domain.Entities;

namespace Example.Domain.Handlers;
public class UserUpdateHandler : IEventHandler<UserUpdateEvent, User>
{
    public void Handle(User projection, UserUpdateEvent e)
    {
        projection.Id = e.Id;
        projection.Name = e.Name;
        projection.Email = e.Email;
    }
}
