using System.Security.Claims;
using BasketService.Application.Model;
using Microsoft.AspNetCore.Http;

namespace BasketService.Application.Service;

public class AuthService : IAuthService
{
    private readonly IHttpContextAccessor _contextAccessor;

    public AuthService(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }
    
    
    public AuthenticatedUser GetAuthenticatedUser()
    {
        var userId = this._contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        var name = this._contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
        var lastName = this._contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Surname)?.Value;
        var email = this._contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
        
        if (!this.IsUserUserCredentialsValid(userId, name, lastName, email))
            return default(AuthenticatedUser);

        return new()
        {
            UserId = userId,
            Name = name,
            LastName = lastName,
            Email = email
        };
    }


    private bool IsUserUserCredentialsValid(string userId, string name, string lastName, string email)
    {
        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(email))
            return false;
        return true;
    }
}