using Example.Domain.Models;

namespace Example.Domain.Events;

public class OrderCreateEvent : OrderEvent
{
    public override Guid StreamId => Id;
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public decimal TotalAmount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public List<Product> Products { get; set; } = [];
}
