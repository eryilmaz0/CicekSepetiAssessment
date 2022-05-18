using AuthenticationService.API.Entity;

namespace AuthenticationService.API.Resources;

public class UserResource
{
    private static List<User> _users;

    public static void InitializeUserData()
    {
        _users = new List<User>()
        {
            new("Eren", "Yılmaz", "eryilmaz0@hotmail.com", "123456"),
            new("User 1", "User 1", "user1@hotmail.com", "123456"),
            new("User 2", "User 2", "user2@hotmail.com", "123456"),
            new("User 3", "User 3", "user3@hotmail.com", "123456"),
        };
    }

    public static List<User> GetUsers()
    {
        return _users;
    }
}