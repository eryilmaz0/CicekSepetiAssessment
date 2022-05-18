using AuthenticationService.API.Models;

namespace AuthenticationService.API.Services;

public interface IAuthenticationService
{
    public Task<LoginResponse> LoginAsync(LoginRequest request);
}