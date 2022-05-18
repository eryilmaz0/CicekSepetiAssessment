using AuthenticationService.API.Entity;
using AuthenticationService.API.Resources;

namespace AuthenticationService.API.Repository;

public class UserRepository : IUserRepository
{
    public User GetUserByEmail(string email)
    {
        return UserResource.GetUsers().FirstOrDefault(user => user.Email.Equals(email));
    }
}