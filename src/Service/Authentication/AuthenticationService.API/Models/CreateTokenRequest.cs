using AuthenticationService.API.Entity;

namespace AuthenticationService.API.Models;

public class CreateTokenRequest
{
    public User User { get; set; }
}