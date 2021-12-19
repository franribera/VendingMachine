using System.Reflection;

namespace Api.Domain.ValueObjects;

public class Coin : ValueObject
{
    public static Coin Zero => new(0);
    public static Coin FiveCent => new(5);
    public static Coin TenCent => new(10);
    public static Coin TwentyCent => new(20);
    public static Coin FiftyCent => new(50);
    public static Coin HundredCent => new(100);

    public int Value { get; }

    private Coin(int value)
    {
        Value = value;
    }

    public static bool IsAllowed(int value)
    {
        var allowedValues = typeof(Coin)
            .GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .Where(p => p.PropertyType == typeof(Coin))
            .Select(p => p.GetValue(null))
            .Cast<Coin>()
            .Select(c => c.Value);

        return allowedValues.Contains(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}