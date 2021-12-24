using Api.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Products.Update;

public class UpdateProductRequest : IRequest<UpdateProductResponse>
{
    public long UserId { get; set; }
    public long ProductId { get; set; }
    public int Quantity { get; set; }
    public int Price { get; set; }
}

public class UpdateProductResponse
{
    public long Id { get; }
    public string Name { get; }
    public int Quantity { get; }
    public int Price { get; }

    public UpdateProductResponse(long id, string name, int quantity, int price)
    {
        Id = id;
        Name = name;
        Quantity = quantity;
        Price = price;
    }
}

public class UpdateProductRequestHandler : IRequestHandler<UpdateProductRequest, UpdateProductResponse>
{
    private readonly VendingMachineDbContext _dbContext;

    public UpdateProductRequestHandler(VendingMachineDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UpdateProductResponse> Handle(UpdateProductRequest request, CancellationToken cancellationToken)
    {
        var product = await _dbContext.Products.SingleOrDefaultAsync(p => p.Id == request.ProductId && p.SellerId == request.UserId, cancellationToken);

        if (product == null)
            throw new KeyNotFoundException($"Product id {request.ProductId} does not exist or you don't have rights to change it.");

        product.Replenish(request.Quantity);
        product.Reprice(request.Price);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new UpdateProductResponse(product.Id, product.Name, product.Quantity, product.Price.Amount);
    }
}