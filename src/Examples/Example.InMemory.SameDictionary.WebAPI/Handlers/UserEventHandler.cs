using Example.Domain.Entities;
using Example.Domain.Events;
using Example.Domain.Handlers;
using SimpleEventSourcing;
using SimpleEventSourcing.Interfaces;

namespace Example.InMemory.SameDictionary.WebAPI.Handlers;

public class UserEventHandler : IEventHandler<Event, User>
{
    public void Handle(User projection, Event e)
    {
        switch (e)
        {
            case UserCreateEvent userCreateEvent:
                new UserCreateHandler().Handle(projection, userCreateEvent);
                break;
            case UserUpdateEvent userUpdateEvent:
                new UserUpdateHandler().Handle(projection, userUpdateEvent);
                break;
            default:
                throw new NotImplementedException();
        }
    }
}
