namespace Example.Domain.Models.Requests;

public class UserUpdateRQ
{
    public required string Name { get; init; }
    public required string Email { get; init; }
}
