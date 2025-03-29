using Example.Domain.Entities;
using Example.Domain.Events;
using Example.Domain.Models.Requests;
using Example.Domain.Models.Response;
using SimpleEventSourcing;

namespace Example.InMemory.DifferentDictionary.WebApi.Services;

public class UserService
{
    private readonly EventSource<User, UserEvent> _userEventSource;

    public UserService(EventSource<User, UserEvent> userEventSource)
    {
        _userEventSource = userEventSource;
    }

    public async Task<UserRS> CreateUserAsync(UserCreateRQ userCreateRQ, CancellationToken cancellationToken)
    {
        var userCreateEvent = new UserCreateEvent()
        {
            UserId = Guid.NewGuid(),
            Email = userCreateRQ.Email,
            Name = userCreateRQ.Name,
            Password = userCreateRQ.Password,
        };

        await _userEventSource.AppendAsync(userCreateEvent, cancellationToken);

        return new UserRS()
        {
            Id = userCreateEvent.UserId,
            Email = userCreateEvent.Email,
            Name = userCreateEvent.Name,
        };
    }

    public async Task<UserRS?> UpdateUserAsync(Guid id, UserUpdateRQ userUpdateRQ, CancellationToken cancellationToken)
    {
        if (!(await _userEventSource.HasProjectionAsync(id, cancellationToken)))
            return null;

        var userUpdateEvent = new UserUpdateEvent()
        {
            Id = id,
            Email = userUpdateRQ.Email,
            Name = userUpdateRQ.Name,
        };

        await _userEventSource.AppendAsync(userUpdateEvent, cancellationToken);

        return new UserRS()
        {
            Id = id,
            Name = userUpdateEvent.Name,
            Email = userUpdateEvent.Email,
        };
    }

    public async Task<User?> GetAsync(Guid id, CancellationToken cancellationToken)
        => await _userEventSource.GetAsync(id, cancellationToken);

    public async Task<List<UserEvent>?> GetEventsAsync(Guid id, CancellationToken cancellationToken)
        => await _userEventSource.GetEventsByStreamIdAsync(id, cancellationToken);

    public async Task<List<UserEvent>> GetAllAsync(CancellationToken cancellationToken)
        => await _userEventSource.GetAllAsync(cancellationToken);
}
