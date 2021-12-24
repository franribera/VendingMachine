namespace Api.Domain.ValueObjects;

public class Money : ValueObject
{
    public static Money None => new(0);

    public int Amount { get; protected set; }

    protected Money() {}

    public Money(int amount)
    {
        Amount = amount;
    }

    public static Money operator +(Money left, Money right)
    {
        return new Money(left.Amount + right.Amount);
    }
    
    public static Money operator -(Money left, Money right)
    {
        return new Money(left.Amount - right.Amount);
    }

    public static Money operator +(Money money, Coin coin)
    {
        return new Money(money.Amount + coin.Value);
    }

    public static Money operator *(Money money, int multiplier)
    {
        return new Money(money.Amount * multiplier);
    }

    public IEnumerable<Coin> Allocate()
    {
        var coins = new List<Coin>();

        var amount = Amount;

        var hundredCentCoins = AllocateTo(amount, Coin.HundredCent);
        coins.AddRange(hundredCentCoins);
        amount -= hundredCentCoins.Sum(c => c.Value);

        var fiftyCentCoins = AllocateTo(amount, Coin.FiftyCent);
        coins.AddRange(fiftyCentCoins);
        amount -= fiftyCentCoins.Sum(c => c.Value);

        var twentyCentCoins = AllocateTo(amount, Coin.TwentyCent);
        coins.AddRange(twentyCentCoins);
        amount -= twentyCentCoins.Sum(c => c.Value);

        var tenCentCoins = AllocateTo(amount, Coin.TenCent);
        coins.AddRange(tenCentCoins);
        amount -= tenCentCoins.Sum(c => c.Value);

        var fiveCentCoins = AllocateTo(amount, Coin.FiveCent);
        coins.AddRange(fiveCentCoins);

        return coins;
    }

    private static IList<Coin> AllocateTo(int amount, Coin coin)
    {
        var hundredCentCount = amount / coin.Value;
        return Enumerable.Repeat(coin, hundredCentCount).ToList();
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Amount;
    }
}