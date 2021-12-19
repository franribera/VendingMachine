namespace Api.Domain.ValueObjects;

public class Username : ValueObject
{
    public string Value { get; protected set; }

    protected Username() { }

    public Username(string value) : this()
    {
        if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Username cannot be empty.", nameof(Username));

        Value = value;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}