namespace BasketService.Application.Model;

public class AuthenticatedUser
{
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
}