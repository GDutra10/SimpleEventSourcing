using Example.Domain.Entities;
using Example.Domain.Events;
using SimpleEventSourcing;

namespace Example.InMemory.SameDictionary.WebAPI;

public class StorageService
{
    private readonly EventSource<User, Event> _userEventSource;
    private readonly EventSource<Order, Event> _orderEventSource;

    public StorageService(EventSource<User, Event> userEventSource, EventSource<Order, Event> orderEventSource)
    {
        _userEventSource = userEventSource;
        _orderEventSource = orderEventSource;
    }

    public async Task SaveEventsAsync(CancellationToken cancellationToken)
    {

        var userId = Guid.NewGuid();
        var tasks = new List<Task>()
        {
            _userEventSource.AppendAsync(new UserCreateEvent()
            {
                Email = "",
                Name = "",
                Password = "",
                UserId = userId,
            }, cancellationToken),
            _orderEventSource.AppendAsync(new OrderCreateEvent()
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Currency = "USD",
                Products = [],
                TotalAmount = 0,
            }, cancellationToken)
        };

        await Task.WhenAll(tasks);
    }

    public async Task<List<Event>> GetAllUserEventsAsync(CancellationToken cancellationToken)
        => await _userEventSource.GetAllAsync(cancellationToken);

    public async Task<List<Event>> GetAllOrderEventsAsync(CancellationToken cancellationToken)
        => await _orderEventSource.GetAllAsync(cancellationToken);
}
