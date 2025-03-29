using Example.Domain.Events;
using Example.Domain.Models;
using SimpleEventSourcing.Interfaces;

namespace Example.Domain.Entities;

public class Order : IProjection
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public decimal TotalAmount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public List<Product> Products { get; set; } = [];
}
