namespace Api.Domain.Entities;

public class Product : Entity
{
    public string Name { get; protected set; }
    public long SellerId { get; protected set; }

    protected Product() { }

    public Product(string name, long sellerId)
    {
        Name = name;
        SellerId = sellerId;
    }
}