namespace Example.Domain.Models.Response;

public class UserRS
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string Email { get; init; }
}
