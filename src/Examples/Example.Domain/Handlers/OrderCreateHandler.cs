using Example.Domain.Entities;
using Example.Domain.Events;
using SimpleEventSourcing.Interfaces;

namespace Example.Domain.Handlers;
public class OrderCreateHandler : IEventHandler<OrderCreateEvent, Order>
{
    public void Handle(Order projection, OrderCreateEvent e)
    {
        projection.Id = e.Id;
        projection.UserId = e.UserId;
        projection.TotalAmount = e.TotalAmount;
        projection.Currency = e.Currency;
        projection.Products = e.Products;
    }
}
