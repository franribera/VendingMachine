using Api.Domain.ValueObjects;

namespace Api.Domain.Entities;

public class Stock : Entity
{
    public long ProductId { get; protected set; }
    public int Quantity { get; protected set; }
    public Money Price { get; protected set; }

    protected Stock() { }

    public Stock(long productId, int quantity, int price)
    {
        ProductId = productId;
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
            throw new InvalidOperationException($"Product {ProductId} understocked. Units left: {Quantity}.");

        Quantity -= quantity;

        return Price * quantity;
    }

    public void Reprice(int price)
    {
        Price = new Money(price);
    }
}