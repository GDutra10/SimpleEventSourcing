namespace Example.Domain.Models.Requests;

public class UserCreateRQ
{
    public required string Name { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
}
