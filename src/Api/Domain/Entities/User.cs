using Api.Domain.Enumerations;
using Api.Domain.ValueObjects;

namespace Api.Domain.Entities;

public class User : Entity
{
    private int _roleId;

    public string Username { get; protected set; }
    public Password Password { get; protected set; }
    public Money Deposit { get; protected set; }
    public Role Role => Enumeration.FromId<Role>(_roleId);

    protected User() { }

    public User(string username, string password, string role) : this()
    {
        Username = username;
        Password = new Password(password);
        Deposit = Money.None;
        _roleId = Enumeration.FromName<Role>(role).Id;
    }

    public void UpdatePassword(string password)
    {
        Password = new Password(password);
    }

    public void DepositMoney(Coin coin)
    {
        Deposit += coin;
    }

    public void Withdraw(Money money)
    {
        var remainingMoney = Deposit - money;

        if (remainingMoney.Amount < 0)
            throw new InvalidOperationException($"Not enough money. Deposit = {Deposit.Amount}");

        Deposit -= money;
    }

    public IEnumerable<Coin> ResetDeposit()
    {
        var withdrawal = Deposit.Allocate();

        Deposit = Money.None;

        return withdrawal;
    }
}