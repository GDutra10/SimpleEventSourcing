namespace Example.Domain.Models.Response;

public class OrderRS
{
    public Guid UserId { get; set; }
    public string Currency { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public List<Product> Products { get; set; } = [];
}
