using Example.Domain.Entities;
using Example.Domain.Events;
using Example.Domain.Handlers;
using SimpleEventSourcing.Interfaces;

namespace Example.InMemory.DifferentDictionary.WebApi.Handlers;

public class UserEventHandler : IEventHandler<UserEvent, User>
{
    private readonly UserCreateHandler _createHandler;
    private readonly UserUpdateHandler _updateHandler;

    public UserEventHandler(UserCreateHandler createHandler, UserUpdateHandler updateHandler)
    {
        _createHandler = createHandler;
        _updateHandler = updateHandler;
    }

    public void Handle(User projection, UserEvent e)
    {
        switch (e)
        {
            case UserCreateEvent userCreateEvent:
                _createHandler.Handle(projection, userCreateEvent);
                break;
            case UserUpdateEvent userUpdateEvent:
                _updateHandler.Handle(projection, userUpdateEvent);
                break;
            default:
                throw new NotImplementedException();
        }
    }
}
