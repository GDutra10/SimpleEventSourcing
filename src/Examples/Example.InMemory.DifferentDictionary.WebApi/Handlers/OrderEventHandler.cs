using Example.Domain.Entities;
using Example.Domain.Events;
using Example.Domain.Handlers;
using SimpleEventSourcing.Interfaces;

namespace Example.InMemory.DifferentDictionary.WebApi.Handlers;

public class OrderEventHandler : IEventHandler<OrderEvent, Order>
{
    private readonly OrderCreateHandler _createHandler;

    public OrderEventHandler(OrderCreateHandler createHandler)
    {
        _createHandler = createHandler;
    }

    public void Handle(Order projection, OrderEvent e)
    {
        if (e is OrderCreateEvent orderCreateEvent)
        {
            _createHandler.Handle(projection, orderCreateEvent);
        }
        else
        {
            throw new NotImplementedException();
        }
    }
}
