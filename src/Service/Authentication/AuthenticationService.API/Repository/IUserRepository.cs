using AuthenticationService.API.Entity;

namespace AuthenticationService.API.Repository;

public interface IUserRepository
{
    public User GetUserByEmail(string email);
}