using Example.Domain.Events;
using SimpleEventSourcing.Interfaces;
using Example.Domain.Entities;

namespace Example.Domain.Handlers;
public class UserCreateHandler : IEventHandler<UserCreateEvent, User>
{
    public void Handle(User projection, UserCreateEvent e)
    {
        projection.Id = e.UserId;
        projection.Name = e.Name;
        projection.Email = e.Email;
        projection.Password = e.Password;
    }
}
