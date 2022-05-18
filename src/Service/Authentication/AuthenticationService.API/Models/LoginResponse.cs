namespace AuthenticationService.API.Models;

public class LoginResponse
{
    public bool IsSuccess { get; set; }
    public string ResultMessage { get; set; }
    public string Token { get; set; }
}