using Example.Domain.Entities;
using Example.Domain.Events;
using Example.Domain.Handlers;
using SimpleEventSourcing;
using SimpleEventSourcing.Interfaces;

namespace Example.InMemory.SameDictionary.WebAPI.Handlers;

public class OrderEventHandler : IEventHandler<Event, Order>
{
    public void Handle(Order projection, Event e)
    {
        if (e is OrderCreateEvent orderCreateEvent)
        {
            new OrderCreateHandler().Handle(projection, orderCreateEvent);
        }
        else
        {
            throw new NotImplementedException();
        }
    }
}
