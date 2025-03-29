using Example.Domain.Entities;
using Example.Domain.Events;
using Example.Domain.Models.Requests;
using Example.Domain.Models.Response;
using SimpleEventSourcing;

namespace Example.InMemory.DifferentDictionary.WebApi.Services;

public class OrderService
{
    private readonly EventSource<Order, OrderEvent> _orderEventSource;

    public OrderService(EventSource<Order, OrderEvent> orderEventSource)
    {
        _orderEventSource = orderEventSource;
    }

    public async Task<OrderRS> CreateOrderAsync(OrderCreateRQ createOrderRQ, CancellationToken cancellationToken)
    {
        var orderCreateEvent = new OrderCreateEvent()
        {
            Id = Guid.NewGuid(),
            TotalAmount = createOrderRQ.Products.Sum(p => p.Amount),
            Currency = createOrderRQ.Products.FirstOrDefault()?.Currency ?? string.Empty,
            Products = createOrderRQ.Products,
            UserId = createOrderRQ.UserId,
        };

        await _orderEventSource.AppendAsync(orderCreateEvent, cancellationToken);

        return new OrderRS()
        {
            Products = orderCreateEvent.Products,
            Currency = orderCreateEvent.Currency,
            TotalAmount = orderCreateEvent.TotalAmount,
            UserId = orderCreateEvent.UserId,
        };
    }

    public async Task<List<OrderEvent>> GetAllAsync(CancellationToken cancellationToken)
        => await _orderEventSource.GetAllAsync(cancellationToken);
}
