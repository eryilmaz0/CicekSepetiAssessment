using BasketService.Application.Model;

namespace BasketService.Application.Service;

public interface IAuthService
{
    public AuthenticatedUser GetAuthenticatedUser();
}