namespace Api.Domain.Enumerations;

public class Role : Enumeration
{
    public static Role Seller = new(1, "Seller");
    public static Role Buyer = new(2, "Buyer");

    private Role(int id, string name) : base(id, name)
    {
    }
}