using Duende.IdentityServer.Models;

namespace Api.Domain.ValueObjects;

public class Password : ValueObject
{
    public string? Value { get; protected set; }

    protected Password() { }

    public Password(string value) : this()
    {
        if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Password cannot be empty.", nameof(Password));
        if (value.Length < 6) throw new ArgumentException("Password must have 6 characters at least.", nameof(Password));

        Value = value.Sha256();
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}