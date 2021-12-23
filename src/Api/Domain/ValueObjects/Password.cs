using Duende.IdentityServer.Models;

namespace Api.Domain.ValueObjects;

public class Password : ValueObject
{
    public string Value { get; protected set; }

    protected Password() { }

    public Password(string value) : this()
    {
        Value = value.Sha256();
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}