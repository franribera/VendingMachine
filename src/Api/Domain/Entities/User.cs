using Api.Domain.ValueObjects;

namespace Api.Domain.Entities;

public class User : Entity
{
    public Username Username { get; protected set; }
    public Password Password { get; protected set; }

    protected User() { }

    public User(string username, string password) : this()
    {
        Username = new Username(username);
        Password = new Password(password);
    }
}