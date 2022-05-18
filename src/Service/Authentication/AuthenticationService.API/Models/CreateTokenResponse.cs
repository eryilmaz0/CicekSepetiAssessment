namespace AuthenticationService.API.Models;

public class CreateTokenResponse
{
    public bool IsSuccess { get; set; }
    public string Token { get; set; }
}