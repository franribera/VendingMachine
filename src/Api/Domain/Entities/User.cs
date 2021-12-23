using Api.Domain.Enumerations;
using Api.Domain.ValueObjects;
using Duende.IdentityServer.Models;

namespace Api.Domain.Entities;

public class User : Entity
{
    private int _roleId;

    public string Username { get; protected set; }
    public Password Password { get; protected set; }
    public Role Role => Enumeration.FromId<Role>(_roleId);

    protected User() { }

    public User(string username, string password, string role) : this()
    {
        SetValues(username, password, role);
    }

    public void Update(string username, string password, string role)
    {
        SetValues(username, password, role);
    }

    private void SetValues(string username, string password, string role)
    {
        Username = username;
        Password = new Password(password);
        _roleId = Enumeration.FromName<Role>(role).Id;
    }
}