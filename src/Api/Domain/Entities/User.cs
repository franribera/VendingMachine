using Api.Domain.Enumerations;
using Api.Domain.ValueObjects;

namespace Api.Domain.Entities;

public class User : Entity
{
    private int _roleId;

    public Username Username { get; protected set; }
    public Password Password { get; protected set; }
    public Role Role => Enumeration.FromId<Role>(_roleId);

    protected User() { }

    public User(string username, string password, string role) : this()
    {
        Username = new Username(username);
        Password = new Password(password);
        _roleId = Enumeration.FromName<Role>(role).Id;
    }

    public void Update(string username, string password, string role)
    {
        if(!string.IsNullOrWhiteSpace(username))
            Username = new Username(username);

        if (!string.IsNullOrWhiteSpace(password))
            Password = new Password(password);

        if (!string.IsNullOrWhiteSpace(role))
            _roleId = Enumeration.FromName<Role>(role).Id;
    }
}