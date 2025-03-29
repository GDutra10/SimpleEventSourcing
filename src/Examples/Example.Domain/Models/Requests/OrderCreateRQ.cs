namespace Example.Domain.Models.Requests;

public class OrderCreateRQ
{
    public Guid UserId { get; set; }
    public List<Product> Products { get; set; } = [];
}
