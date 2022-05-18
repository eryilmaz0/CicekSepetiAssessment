using AuthenticationService.API.Models;

namespace AuthenticationService.API.Services;

public interface ITokenService
{
    public Task<CreateTokenResponse> CreateTokenAsync(CreateTokenRequest request);
}