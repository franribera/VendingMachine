using Api.Domain.ValueObjects;

namespace Api.Domain.Entities;

public class Product : Entity
{
    public string Name { get; protected set; }
    public long SellerId { get; protected set; }
    public int Quantity { get; protected set; }
    public Money Price { get; protected set; }

    protected Product() { }

    public Product(string name, long sellerId, int quantity, int price)
    {
        Name = name;
        SellerId = sellerId;
        Quantity = quantity;
        Price = new Money(price);
    }
    
    public void Replenish(int quantity)
    {
        Quantity += quantity;
    }

    public Money Take(int quantity)
    {
        if (Quantity - quantity < 0)
            throw new InvalidOperationException($"Product {Id} understocked. Units left: {Quantity}.");

        Quantity -= quantity;

        return Price * quantity;
    }

    public void Reprice(int price)
    {
        Price = new Money(price);
    }
}