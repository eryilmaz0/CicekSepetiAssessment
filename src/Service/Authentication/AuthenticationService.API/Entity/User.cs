namespace AuthenticationService.API.Entity;

public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }


    public User(string name, string lastName, string email, string password)
    {
        this.Id = Guid.NewGuid();
        this.Name = name;
        this.LastName = lastName;
        this.Email = email;
        this.Password = password;
    }

    public User()
    {
        
    }
}